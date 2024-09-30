using FluentValidation;
using MongoDB.Bson;
using War.Server.Domain;

namespace War.Server.Models.Forms
{
    public record PasswordResetForm
    {
        public required ObjectId RequestId { get; init; }
        public required string Key { get; init; }
        public required string Password { get; init; }
        public required string ConfirmPassword { get; init; }

        public class Validator : AbstractValidator<PasswordResetForm>
        {
            public Validator() {
                RuleFor(x => x.Key).NotEmpty().WithMessage("Key required.");
                RuleFor(x => x.Password).NotEmpty().WithMessage("Password required.");
                RuleFor(x => x.ConfirmPassword).Matches(x => x.Password).WithMessage("Passwords does not match.").When(x => !string.IsNullOrEmpty(x.Password));
            }
        }
    }
}
