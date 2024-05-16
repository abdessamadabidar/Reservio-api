using Reservio.Dto;
using Reservio.Models;


namespace Reservio.Interfaces
{
    public interface INotificationService
    {
        public ICollection<NotificationResponseDto> GetAllNotifications();
        public NotificationResponseDto GetNotificationById(Guid id);
        public bool AddNotification(NotificationRequestDto notification);
        public bool UpdateNotification(NotificationRequestDto notification);
        public bool DeleteNotification(Guid id);
        public bool NotificationExists(Guid id);
        public bool MarkNotificationAsRead(Guid id);
        public bool MarkNotificationAsUnread(Guid id);
    }
}
