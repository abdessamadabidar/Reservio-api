using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reservio.Models
{
    public class Reservation
    {
        [Required]
        public DateTime StartDateTime { get; set; }
        [Required]
        public DateTime EndDateTime { get; set; }
        [Required]
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }

        [ForeignKey("RoomId")]
        public int RoomId { get; set; }
        public User User { get; set; } = null!;
        public Room Room { get; set; } = null!;
    }
}
