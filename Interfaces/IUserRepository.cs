using Reservio.Models;

namespace Reservio.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetAllUsers();
        User GetUserById(int id);
        IEnumerable<User> GetUsersByRole(string roleName);
        bool UserExists(int id);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool Save();
    }
}
