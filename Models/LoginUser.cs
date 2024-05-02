using Microsoft.AspNetCore.Identity;

namespace Reservio.Models
{
    public class LoginUser
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
