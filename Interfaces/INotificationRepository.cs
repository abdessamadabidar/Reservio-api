using Reservio.Models;

namespace Reservio.Interfaces
{
    public interface INotificationRepository
    {
        ICollection<Notification> GetNotifications();
        Notification GetNotificationById(Guid id);
        bool CreateNotification(Notification notification);
        bool UpdateNotification(Notification notification);
        bool DeleteNotification(Notification notification);
        bool NotificationExists(Guid id);
        bool Save();
    }
}
