using MongoDB.Bson;
using War.Server.Database;
using War.Server.Domain.Exceptions;
using War.Server.Domain.Repositories;

namespace War.Server.Domain.Services
{
    public interface ISessionService
    {
        GameBoard GameBoard { get; }
        Token Token { get; }
        User User { get; }
        Player? Player { get; }
        Languages Language { get; }
        void Logout();
        void CheckTenant();
        void CheckUser();
    }

    public class SessionService : ISessionService
    {
        readonly ITokenService tokenService;
        readonly IHttpContextService httpContextService;

        Player? player = null;
        Token? token = null;
        GameBoard? gameBoard = null;

        public SessionService(ITokenService tokenService, IHttpContextService httpContextService)
        {
            this.httpContextService = httpContextService;
            this.tokenService = tokenService;
        }

        /// <summary>
        /// Oturuma ait token
        /// </summary>
        public Token Token
        {
            get
            {
                token ??= tokenService.Parse(httpContextService.Token).Result;
                return token;
            }
        }

        /// <summary>
        /// Oturumu açık kullanıcı
        /// </summary>
        public User User { get => Token.User; }

        /// <summary>
        /// İşlem yapılan hesap
        /// </summary>
        public GameBoard GameBoard
        {
            get
            {
                if (gameBoard is null)
                {
                    ObjectId? gameBoardId = httpContextService.GameBoardId;
                    if (gameBoardId.HasValue)
                    {
                        gameBoard = Repository<GameBoard>.Find(gameBoardId.Value);
                        if (gameBoard is null)
                            throw new GameBoardMismatchException("User not in this gameboard.");
                    }
                }

                if (gameBoard is not null && this.User is not null)
                {
                    
                    if (!gameBoard.GetPlayers().Any(x => x.UserId == User.Id))
                        throw new GameBoardMismatchException("User not in this gameboard.");
                }

                return gameBoard ?? throw new GameBoardRequiredException();
            }
        }

        public Player? Player
        {
            get
            {
                if (GameBoard is null || User is null)
                { 
                    player = null;
                }
                else if(player is null)
                {
                    player = Repository<Player>.GetAll().FirstOrDefault(x => x.GameBoardId == GameBoard.Id && x.UserId == User.Id);
                }

                return player;
            }
        }

        public Languages Language
        {
            get
            {
                return User?.Language ?? Languages.Turkish;
            }
        }

        /// <summary>
        /// Mevcut oturumu sonlandırır.
        /// </summary>
        public void Logout()
        {
            if (this.Token != null)
                tokenService.Delete(this.Token);
        }

        public void CheckTenant()
        {
            if (GameBoard is null)
                throw new GameBoardMismatchException("User not in this gameboard.");
        }

        public void CheckUser()
        {
            if (User == null)
                throw new UnauthorizedAccessException("User not found");
        }
    }
}
