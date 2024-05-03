using AutoMapper;
using Reservio.Dto;
using Reservio.Interfaces;
using Reservio.Models;

namespace Reservio.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public NotificationService(INotificationRepository notificationRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public bool AddNotification(NotificationRequestDto notificationRequest)
        {
            var NotificationMap = _mapper.Map<Notification>(notificationRequest);
            return _notificationRepository.CreateNotification(NotificationMap);
        }

        public bool DeleteNotification(Guid id)
        {
            var notificationToDelete = _notificationRepository.GetNotificationById(id);
            if (notificationToDelete == null)
                return false;

            return _notificationRepository.DeleteNotification(notificationToDelete);
           
        }

        public ICollection<NotificationResponseDto> GetAllNotifications()
        {
            var notifications = _mapper.Map<List<NotificationResponseDto>>(_notificationRepository.GetNotifications());
            return notifications;
        }

        public NotificationResponseDto GetNotificationById(Guid id)
        {
            var notification = _mapper.Map<NotificationResponseDto>(_notificationRepository.GetNotificationById(id));
            return notification;
        }

        public bool NotificationExists(Guid id)
        {
            return _notificationRepository.NotificationExists(id);
        }

        public bool UpdateNotification(NotificationRequestDto notificationRequest)
        {
            var notificationToUpdate = _mapper.Map<Notification>(notificationRequest);
            return _notificationRepository.UpdateNotification(notificationToUpdate);
        }
    }
}
