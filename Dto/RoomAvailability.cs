namespace Reservio.Dto
{
    public class RoomAvailability
    {
        public Guid RoomId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
