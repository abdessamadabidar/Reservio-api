using System.ComponentModel.DataAnnotations;

namespace Reservio.Dto {
    public class RoomDto {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public bool isReserved { get; set; }

    }
}
