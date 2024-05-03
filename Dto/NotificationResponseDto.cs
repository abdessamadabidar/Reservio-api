using Reservio.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reservio.Dto
{
    public class NotificationResponseDto
    {
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

    }
}
