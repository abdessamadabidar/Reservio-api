using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Reservio.Models
{
    public class Equipment
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<RoomEquipment> RoomEquipments { get; set; } =  new List<RoomEquipment>();
    }
}
