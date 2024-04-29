using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Reservio.Dto;
using Reservio.Interfaces;
using Reservio.Models;

namespace Reservio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<UserDto>))]
        public IActionResult GetAllUsers()
        {
            var users = _mapper.Map<List<UserDto>>(_userRepository.GetAllUsers());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(users);
        }

        [HttpGet("{UserId:int}")]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        [ProducesResponseType(400)]
        public IActionResult GetUserById(int UserId)
        {
            if (!_userRepository.UserExists(UserId))
                return NotFound();

            var user = _mapper.Map<UserDto>(_userRepository.GetUserById(UserId));
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

            var users = _mapper.Map<IEnumerable<UserDto>>(_userRepository.GetUsersByRole(Role));
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

            var user = _userRepository
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

            
            var userMap = _mapper.Map<User>(newUserDto);

            if (!_userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", $"Something went wrong saving the user {userMap.Email}");
                return StatusCode(500, ModelState);
            }

            return Ok("User created successfully");
        }

        [HttpPut("{UserId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUser(int UserId, [FromBody] UserDto updatedUserDto)
        {
            if (updatedUserDto == null || UserId != updatedUserDto.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_userRepository.UserExists(UserId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var userMap = _mapper.Map<User>(updatedUserDto);

            if (!_userRepository.UpdateUser(userMap))
            {
                ModelState.AddModelError("", $"Something went wrong updating the user {userMap.Email}");
                return StatusCode(500, ModelState);
            }

            return Ok("User updated successfully");
        }

        [HttpDelete("{UserId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUser(int UserId)
        {
            if (!_userRepository.UserExists(UserId))
            {
                return NotFound();
            }

            var userToDelete = _userRepository.GetUserById(UserId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_userRepository.DeleteUser(userToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting the user {userToDelete.Email}");
                return StatusCode(500, ModelState);
            }

            return Ok("User deleted successfully");
        }
       
    }
}
