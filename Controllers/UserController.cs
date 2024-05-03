using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reservio.Dto;
using Reservio.Interfaces;


namespace Reservio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<UserDto>))]
        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "USER")]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(users);
        }

        [HttpGet("{UserId:guid}")]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        [ProducesResponseType(400)]
        public IActionResult GetUserById(Guid UserId)
        {
            if (!_userService.UserExists(UserId))
                return NotFound();

            var user = _userService.GetUserById(UserId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(user);
        }

        [HttpGet("role/{Role:alpha}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetUserByRole(string Role)
        {

            var users = _userService.GetUsersByRole(Role);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(users);
        }

        

        [HttpPut("{UserId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUser(Guid UserId, [FromBody] UserDto updatedUserDto)
        {
            if (updatedUserDto == null || UserId != updatedUserDto.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_userService.UserExists(UserId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            

            if (!_userService.UpdateUser(updatedUserDto))
            {
                ModelState.AddModelError("", $"Something went wrong updating the user {updatedUserDto.Email}");
                return StatusCode(500, ModelState);
            }

            return Ok("User updated successfully");
        }

        [HttpDelete("{UserId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUser(Guid UserId)
        {
            if (!_userService.UserExists(UserId))
            {
                return NotFound();
            }

            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_userService.DeleteUser(UserId))
            {
                ModelState.AddModelError("", "Something went wrong deleting the user");
                return StatusCode(500, ModelState);
            }

            return Ok("User deleted successfully");
        }

        [HttpGet("{userId:guid}/notifications")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<NotificationResponseDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetUserNotifications(Guid userId)
        {
            if (!_userService.UserExists(userId))
            {
                return NotFound("user not found");
            }

            var notifications = _userService.GetAllNotifications(userId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(notifications);
        }


        [HttpGet("{userId:guid}/roles")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<NotificationResponseDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetUserRoles(Guid userId)
        {
            if (!_userService.UserExists(userId))
            {
                return NotFound("user not found");
            }

            var roles = _userService.GetUserRoles(userId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(roles);
        }
    }
}
