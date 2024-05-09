using System.ComponentModel.DataAnnotations;

namespace Reservio.Dto
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}
