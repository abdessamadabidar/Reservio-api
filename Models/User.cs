using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Reservio.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        public bool IsApproved { get; set; } = false;
        public bool IsActivated { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<RoleUser> UserRoles { get; set; }

   
    }
}
