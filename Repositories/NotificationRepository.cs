using Reservio.Data;
using Reservio.Interfaces;
using Reservio.Models;

namespace Reservio.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public bool CreateNotification(Notification notification)
        {
            _context.Notifications.Add(notification);
            return Save();
        }

        public bool DeleteNotification(Notification notification)
        {
            _context.Notifications.Remove(notification);
            return Save();
        }

        public Notification GetNotificationById(Guid id)
        {
            return _context.Notifications.Find(id);
        }

        public ICollection<Notification> GetNotifications()
        {
            return _context.Notifications
                .OrderByDescending(notif => notif.CreatedAt)
                .ToList();
        }

        public bool NotificationExists(Guid id)
        {
            return _context.Notifications.Any(notif => notif.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }


        public bool UpdateNotification(Notification notification)
        {
            _context.Notifications.Update(notification);
            return Save();
        }
    }
}
