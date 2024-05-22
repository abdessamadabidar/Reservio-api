using Reservio.Models;

namespace Reservio.Dto
{
    public class ReservationResponseDto
    {
        public Guid Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string? Description { get; set; }
        public Guid UserId { get; set; }
        public UserDto User { get; set; }
        public Guid RoomId { get; set; }
        public RoomResponseDto Room { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
