using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reservio.Models
{
    public class RoleUser
    {
  
        [ForeignKey("RoleId")]
        public Guid RoleId { get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
        public Role Role { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
