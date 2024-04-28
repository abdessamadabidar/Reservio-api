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

        [HttpGet("{UserId}")]
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
    }
}
