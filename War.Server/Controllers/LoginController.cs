using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using War.Server.Domain;
using War.Server.Domain.Exceptions;
using War.Server.Domain.Services;
using War.Server.Models.Forms;
using War.Server.Models.Results;

namespace War.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        readonly ISessionService sessionService;
        readonly UserService userService;

        public LoginController(ISessionService sessionService, UserService userService)
        {
            this.sessionService = sessionService;
            this.userService = userService;
        }

        [HttpPost]
        public async Task<TokenResult> Post(LoginForm form)
        {
            var token = await userService.Login(form);

            if (token == null)
                throw new UserException("Login failed.");

            return new TokenResult(token);
        }

        [HttpGet]
        public TokenResult Get()
        {
            var token = sessionService.Token;
            return new TokenResult(token);
        }
    }
}
