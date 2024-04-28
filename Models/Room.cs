using System.ComponentModel.DataAnnotations;

namespace Reservio.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; } = null!;
        public ICollection<Reservation> Reservations { get; set; }
    }
}
