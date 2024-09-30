using War.Server.Domain.Exceptions;
using War.Server.Domain.Repositories;

namespace War.Server.Domain.Services
{
    public interface ITokenService
    {
        void Delete(Token token);
        Task<Token> Generate(User user, Token.TokenSource source = Token.TokenSource.Web, Token.TokenKind kind = Token.TokenKind.Password);
        Task<Token> Parse(string? key);
    }

    public class TokenService : ITokenService
    {
        readonly IHttpContextService httpContextService;

        public TokenService(IHttpContextService httpContextService)
        {
            this.httpContextService = httpContextService;
        }

        public async Task<Token> Parse(string? key)
        {
            if (key == null)
                throw new InvalidTokenException("Invalid token.");

            var token = Repository<Token>.GetAll().FirstOrDefault(x => x.Key == key);

            if (token == null)
                throw new InvalidTokenException("Invalid token.");

            if (!token.IsValid())
                throw new InvalidTokenException("Token expired.");

            await Repository<Token>.SaveAsync(token);

            return token;
        }

        /// <summary>
        /// Kullanıcı için yeni token oluşturur.
        /// </summary>
        /// <param name="user">Kullanıcı</param>
        /// <param name="source">Token talep eden uygulama</param>
        /// <param name="kind">Oturum açma türü</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<Token> Generate(User user, Token.TokenSource source = Token.TokenSource.Web, Token.TokenKind kind = Token.TokenKind.Password)
        {
            ArgumentNullException.ThrowIfNull(nameof(user));


            var t = new Token(user, source, kind);

            t.SetHttpContextInfo(httpContextService);
            await Repository<Token>.SaveAsync(t);

            if (kind != Token.TokenKind.Maintenance)
            {
                // Bu konumdan açılmış geçmiş oturumları sonlandır.
                var oldTokens = Repository<Token>.GetAll().Where(x => x.UserId == user.Id && x.Source == source && x.Kind != Token.TokenKind.Maintenance && x.Id != t.Id && x.LastValidDate >= DateTime.UtcNow);
                foreach (var o in oldTokens)
                {
                    Repository<Token>.Delete(o);
                }
            }

            return t;
        }

        public void Delete(Token token)
        {
            Repository<Token>.Delete(token);
        }
    }
}
