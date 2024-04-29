using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reservio.Models
{
    public class RoleUser
    {
  
        [ForeignKey("RoleId")]
        public int RoleId { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public Role Role { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
