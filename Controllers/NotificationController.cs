using Microsoft.AspNetCore.Mvc;
using Reservio.Dto;
using Reservio.Interfaces;
using Reservio.Models;

namespace Reservio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;

        public NotificationController(INotificationService notificationService, IUserService userService)
        {
            _notificationService = notificationService;
            _userService = userService;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Notification>))]
        [ProducesResponseType(400)]
        public IActionResult GetAllNotifications()
        {
            var notifications = _notificationService.GetAllNotifications();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(notifications);
        }


        [HttpGet("{NotificationId:guid}")]
        [ProducesResponseType(200, Type = typeof(Notification))]
        [ProducesResponseType(400)]
        public IActionResult GetNotificationById(Guid NotificationId)
        {
            if (!_notificationService.NotificationExists(NotificationId))
                return NotFound();

            var notification = _notificationService.GetNotificationById(NotificationId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(notification);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Notification))]
        [ProducesResponseType(400)]
        public IActionResult CreateNotification([FromBody] NotificationRequestDto notification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_userService.UserExists(notification.UserId))
                return NotFound("User with provided Id not found");

            if (notification == null)
                return BadRequest("Invalid notification object");

            if (!_notificationService.AddNotification(notification))
            {
                ModelState.AddModelError("Notification", "Could not add notification");
                return StatusCode(500, ModelState);
            }

            return Ok("Notifcation has been created successfully");
        }


        [HttpDelete("{NotificationId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteNotification(Guid NotificationId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_notificationService.NotificationExists(NotificationId))
                return NotFound("Notification Not found");


            if (!_notificationService.DeleteNotification(NotificationId))
            {
                ModelState.AddModelError("Notification", "Could not delete notification");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        [HttpPut("{NotificationId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateNotification(Guid NotificationId, [FromBody] NotificationRequestDto updatedNotification)
        {
            if (updatedNotification == null || NotificationId != updatedNotification.Id)
            {
                return BadRequest(ModelState);
            }


            if (!_userService.UserExists(updatedNotification.UserId))
                return NotFound("User with provided Id not found");


            if (!_notificationService.NotificationExists(NotificationId))
            {
                return NotFound("Notification not found");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_notificationService.UpdateNotification(updatedNotification))
            {
                ModelState.AddModelError("", $"Something went wrong updating the notification {updatedNotification.Title}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        [HttpPut("{NotificationId:guid}/read")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult MarkNotificationAsRead(Guid NotificationId)
        {
            if (!_notificationService.NotificationExists(NotificationId))
                return NotFound("Notification not found");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (!_notificationService.MarkNotificationAsRead(NotificationId))
            {
                ModelState.AddModelError("Notification", "Could not mark notification as read");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpPut("{NotificationId:guid}/unread")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult MarkNotificationAsUnread(Guid NotificationId)
        {
            if (!_notificationService.NotificationExists(NotificationId))
                return NotFound("Notification not found");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (!_notificationService.MarkNotificationAsUnread(NotificationId))
            {
                ModelState.AddModelError("Notification", "Could not mark notification as read");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpGet("{userId:guid}/unread/count")]
        [ProducesResponseType(200, Type = typeof(int))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CountUnreadNotifications(Guid userId)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userId == null)
            {
                ModelState.AddModelError("Notification", "User Id is null");
                return StatusCode(400, ModelState);
            }

            var count = await _notificationService.UnreadNotificationsCount(userId);
            return Ok(count);
        }
    }
}
