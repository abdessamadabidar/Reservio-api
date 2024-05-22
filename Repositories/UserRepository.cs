using System.Collections.ObjectModel;
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

        public async Task<List<Guid>> GetAdmins()
        {
            var adminsId = await _context
                .RoleUsers
                .Where(ru => ru.Role.Name == "ADMIN")
                .Select(ru => ru.UserId)
                .ToListAsync();

            return adminsId;

        }

        public ICollection<User> GetAllUsers()
        {
            return _context.Users.OrderByDescending(user => user.CreatedAt).ToList();
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(user => user.Email == email);
        }

        public User GetUserById(Guid id)
        {
            return _context.Users.Include(user => user.Reservations).Where(u => u.Id == id).FirstOrDefault();
        }

        public IEnumerable<Notification> GetUserNotifications(Guid userId)
        {
            return _context.Notifications
                .Where(notif => notif.UserId == userId)
                .OrderByDescending(notif => notif.CreatedAt)
                .ToList();
        }

        public IEnumerable<Reservation> GetUserReservations(Guid UserId)
        {
            return _context.Reservations
                .Include(reservation => reservation.Room)
                .Where(reservation => reservation.UserId == UserId)
                .OrderByDescending(reservation => reservation.CreatedAt)
                .ToList();

        }

        public IEnumerable<string> GetUserRoles(Guid userId)
        {
            return from role in _context.Roles
                   join userRole in _context.RoleUsers on role.Id equals userRole.RoleId
                   where userRole.UserId == userId
                   select role.Name;
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

        public async Task<bool> UpdateUser(User user)
        {
            _context.Update(user);
            return Save();
        }

        public bool UserExists(Guid id)
        {
            return _context.Users.Any(user => user.Id == id);
        }


    }
}
