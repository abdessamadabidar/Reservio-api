using System.ComponentModel.DataAnnotations.Schema;
namespace Reservio.Models
{
    public class RoomEquipment
    {
        [ForeignKey("RoomId")]
        public Guid RoomId { get; set; }
        public Room Room { get; set; } = null!;

        [ForeignKey("EquipmentId")]
        public Guid EquipmentId { get; set; }
        public Equipment Equipment { get; set; } = null!;
    }
}
