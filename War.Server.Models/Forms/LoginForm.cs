using War.Server.Domain;
using FluentValidation;

namespace War.Server.Models.Forms
{
    public record LoginForm : ILoginForm
    {
        public required string Email { get; init; }

        public required string Password { get; init; }

        public class Validator : AbstractValidator<LoginForm>
        {
            public Validator() {

                RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Invalid e-mail address.");
                RuleFor(x => x.Password).NotEmpty().WithMessage("Password required.");
            }
        }
    }
}
