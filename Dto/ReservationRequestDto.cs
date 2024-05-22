using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reservio.Dto
{
    public class ReservationRequestDto
    {
        public Guid? Id { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }
        public string? Description { get; set; }
        public Guid UserId { get; set; }
        public Guid RoomId { get; set; }
    }
}
