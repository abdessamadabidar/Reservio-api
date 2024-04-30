using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reservio.Models
{
    public class Reservation : BaseEntity
    {
        [Required]
        public DateTime StartDateTime { get; set; }
        [Required]
        public DateTime EndDateTime { get; set; }
        [Required]
        public string Description { get; set; } = null!;

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }

        [ForeignKey("RoomId")]
        public Guid RoomId { get; set; }
        public User User { get; set; } = null!;
        public Room Room { get; set; } = null!;
    }
}
