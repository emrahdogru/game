namespace War.Server.Models.Forms
{
    public record PasswordResetRequestForm
    {
        public required string Email { get; init; }
    }
}
