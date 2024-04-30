using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reservio.Models
{
    public class User : BaseEntity
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
        public DateTime? VerifiedAt { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<RoleUser> UserRoles { get; set; }

   
        
    }
}
