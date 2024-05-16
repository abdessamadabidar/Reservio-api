namespace Reservio.Dto
{
    public class LoginResponseDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsApproved { get; set; } = false;
        public bool IsActivated { get; set; } = true;
        public string Token { get; set; } = null!;
        public IEnumerable<string> Roles { get; set; } = new List<string>();

        public int? statusCode { get; set; }
        public string? message { get; set; } = null!;
    }
}
