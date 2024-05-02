using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reservio.Models
{
    public class Room : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Code { get; set; } = null!;

        public bool isReserved = false;

        public Room(string code)
        {
            Code = code;
        }
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
