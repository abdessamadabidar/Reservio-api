using Microsoft.AspNetCore.Identity;
using Reservio.Models;

namespace Reservio.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetAllUsers();
        User GetUserById(Guid id);
        IEnumerable<User> GetUsersByRole(string roleName);
        bool UserExists(Guid id);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        User GetUserByEmail(string email);
        IEnumerable<string> GetUserRoles(Guid UserId);
        IEnumerable<Notification> GetUserNotifications(Guid UserId);
        bool Save();
    }
}
