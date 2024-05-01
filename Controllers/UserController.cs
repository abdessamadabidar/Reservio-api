using AutoMapper;
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

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromBody] UserDto newUserDto)
        {
            if (newUserDto == null)
            {
                return BadRequest(ModelState);
            }

            var user = _userService
                .GetAllUsers()
                .Where(user => user.Email == newUserDto.Email)
                .FirstOrDefault();

            if (user != null)
            {
                ModelState.AddModelError("", "Email already exists");
                // 422 The server cannot process the request because it contains invalid properties.
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (!_userService.CreateUser(newUserDto))
            {
                ModelState.AddModelError("", $"Something went wrong saving the user {newUserDto.Email}");
                return StatusCode(500, ModelState);
            }

            return Ok("User created successfully");
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
       
    }
}
