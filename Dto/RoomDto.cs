
using Reservio.Models;

namespace Reservio.Dto
{
    public class RoomDto
    { 
        public string Name { get; set; } = null!;
        public int Capacity { get; set; }
        public string? Description { get; set; }
        public IFormFile? ImageFile { get; set; } = null!;
        public ICollection<EquipmentDto> Equipments { get; set; }
     

    }
}
