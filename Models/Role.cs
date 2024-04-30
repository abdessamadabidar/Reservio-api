using System.ComponentModel.DataAnnotations;

namespace Reservio.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<RoleUser> UserRoles { get; set; }
    }
}
