using Reservio.Data;
using Reservio.Interfaces;
using Reservio.Models;

namespace Reservio.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICollection<User> GetAllUsers()
        {
            return _context.Users.OrderByDescending(user => user.CreatedAt).ToList();
        }

        public User GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public bool UserExists(int id)
        {
            return _context.Users.Any(user => user.Id == id);
        }
    }
}
