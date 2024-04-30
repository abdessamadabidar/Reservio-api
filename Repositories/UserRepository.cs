using Microsoft.EntityFrameworkCore;
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

        public bool CreateUser(User user)
        {
            _context.Add(user);
            return Save();
        }

        public bool DeleteUser(User user)
        {
              _context.Remove(user);
            return Save();
        }

        public ICollection<User> GetAllUsers()
        {
            return _context.Users.OrderByDescending(user => user.CreatedAt).ToList();
        }

        public User GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

       
        public IEnumerable<User> GetUsersByRole(string roleName)
        {
            return from user in _context.Users
                   join userRole in _context.RoleUsers on user.Id equals userRole.UserId
                   join role in _context.Roles on userRole.RoleId equals role.Id
                   where role.Name == roleName
                   select user;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateUser(User user)
        { 
            _context.Update(user);
            return Save();
        }

        public bool UserExists(int id)
        {
            return _context.Users.Any(user => user.Id == id);
        }

        
    }
}
