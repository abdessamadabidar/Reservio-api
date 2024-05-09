using System.ComponentModel.DataAnnotations;

namespace Reservio.Dto {
    public class RoomResponseDto {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Capacity { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
   
    }
}
