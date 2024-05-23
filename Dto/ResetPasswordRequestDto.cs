namespace Reservio.Dto
{
    public class ResetPasswordRequestDto
    {
        public string NewPassword { get; set; } = null!;
        public string NewPasswordConfirmation { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
