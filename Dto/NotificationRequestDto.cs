using Reservio.Models;

namespace Reservio.Dto
{
    public class NotificationRequestDto
    {
        public Guid? Id { get; set; }
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        public Guid UserId { get; set; }
 
    }
}
