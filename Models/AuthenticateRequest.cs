using Microsoft.AspNetCore.Identity;

namespace Reservio.Models
{
    public class AuthenticateRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
