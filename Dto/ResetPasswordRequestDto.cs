namespace Reservio.Dto
{
    public class ResetPasswordRequestDto
    {
        public string Password { get; set; } = null!;
        public string Token { get; set; } = null!; 
    }
}
