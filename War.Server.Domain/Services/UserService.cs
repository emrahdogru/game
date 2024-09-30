using War.Server.Database;
using War.Server.Domain.Exceptions;
using War.Server.Domain.Repositories;
using War.Server.Models.Forms;

namespace War.Server.Domain.Services
{
    public class UserService
    {
        readonly ITokenService tokenService;

        public UserService(ISessionService sessionService, ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        public async Task<Token> Login(ILoginForm form)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));

            var user = FindByEmail(form.Email);
            if (user?.IsValidPassword(form.Password) == true)
            {
                return await tokenService.Generate(user);
            }

            throw new UserException("Login failed.");
        }

        public User? FindByEmail(string email)
        {
            if (email == null)
                throw new ArgumentNullException(nameof(email));

            return UserRepository.FindByEmail(email) ?? throw new EntityNotFountException(nameof(User), email);
        }

        public async Task ChangePassword(User? user, IChangePasswordForm form)
        {
            if (user == null)
                throw new UserAuthorizationException();

            if (form == null)
                throw new ArgumentNullException(nameof(form));

            if (user.IsValidPassword(form.OldPassword))
            {
                user.SetPassword(form.NewPassword);
                await Repository<User>.SaveAsync(user);
            }
            else
            {
                throw new UserException("Login failed.");
            }
        }

        public async Task<PasswordResetRequest> CreatePasswordResetRequest(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var request = new PasswordResetRequest(user, false);


            await Repository<PasswordResetRequest>.SaveAsync(request);

            return request;
        }
    }
}
