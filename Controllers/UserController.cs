using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Encodings;
using Reservio.Dto;
using Reservio.Helper;
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
        [ProducesResponseType(200, Type = typeof(ICollection<UserResponseDto>))]
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
        [ProducesResponseType(200, Type = typeof(UserResponseDto))]
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
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserResponseDto>))]
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
        public async Task<IActionResult> UpdateUser(Guid UserId, [FromBody] UpdateUserDto updatedUserDto)
        {
            if (updatedUserDto == null)
            {
                return BadRequest("user id is null");
            }


            if (!_userService.UserExists(UserId))
            {
                return NotFound("User not found");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }



            if (!await _userService.UpdateUser(updatedUserDto))
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

        [HttpGet("{userId:guid}/reservations")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReservationResponseDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetUserReservations(Guid userId)
        {
            if (!_userService.UserExists(userId))
            {
                return NotFound("user not found");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var reservations = _userService.GetAllReservations(userId);


            return Ok(reservations);

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


        [HttpPut("{userId:guid}/change-password")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ChangePassword(Guid userId, [FromBody] ChangePasswordRequestDto changePasswordRequest)
        {

            if (!ModelState.IsValid || changePasswordRequest == null)
            {
                return BadRequest(ModelState);
            }


            var result = await _userService.ChangePassword(userId, changePasswordRequest);

            if (result == Result.UserNotFound)
            {
                return NotFound("user not found");
            }

            if (result == Result.PasswordNotMatch)
            {
                ModelState.AddModelError("", "Password does not match");
                return BadRequest(ModelState);
            }

            if (result == Result.ChangePasswordFailure)
            {
                ModelState.AddModelError("", "Something went wrong changing the password");
                return StatusCode(500, ModelState);
            }

            return Ok("Password changed successfully");
        }


        [HttpPut("{userId:guid}/enable")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> EnableUser(Guid userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_userService.UserExists(userId))
            {
                return NotFound("user not found");
            }


            if (!await _userService.EnableUser(userId))
            {
                ModelState.AddModelError("user", "Something went wrong enabling the user");
                return StatusCode(500, ModelState);

            }


            return Ok("User enabled successfully");

        }


        [HttpPut("{userId:guid}/disable")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DisableUser(Guid userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_userService.UserExists(userId))
            {
                return NotFound("user not found");
            }


            if (!await _userService.DisableUser(userId))
            {
                ModelState.AddModelError("user", "Something went wrong disabling the user");
                return StatusCode(500, ModelState);

            }


            return Ok("User disabled successfully");

        }



        [HttpPut("{userId:guid}/approve")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ApproveUser(Guid userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_userService.UserExists(userId))
            {
                return NotFound("user not found");
            }

            if (!await _userService.ApproveUser(userId))
            {
                ModelState.AddModelError("user", "Something went wrong approving the user");
                return StatusCode(500, ModelState);
            }
            return Ok("User approved successfully");

        }




    }
}
