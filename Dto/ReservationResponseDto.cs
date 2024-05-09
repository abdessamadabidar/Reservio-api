namespace Reservio.Dto
{
    public class ReservationResponseDto
    {
        public Guid Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string? Description { get; set; }
        public Guid UserId { get; set; }
        public UserDto User { get; set; } = null!;
        public Guid RoomId { get; set; }
        public RoomDto Room { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
