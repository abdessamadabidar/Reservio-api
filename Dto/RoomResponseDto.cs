using Reservio.Models;
using System.ComponentModel.DataAnnotations;

namespace Reservio.Dto {
    public class RoomResponseDto {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Capacity { get; set; }
        public string? ImagePath { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<RoomEquipmentDto> RoomEquipments { get; set; }
        
   
    }
}
