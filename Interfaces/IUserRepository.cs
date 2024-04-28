using Reservio.Models;

namespace Reservio.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetAllUsers();
        User GetUserById(int id);
        bool UserExists(int id);

    }
}
