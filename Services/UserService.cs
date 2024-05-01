using AutoMapper;
using Reservio.Dto;
using Reservio.Interfaces;
using Reservio.Models;

namespace Reservio.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public bool CreateUser(UserDto user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            var userMap = _mapper.Map<User>(user);

            return _userRepository.CreateUser(userMap);
        }

        public bool DeleteUser(Guid userId)
        {
            var userToDelete = _userRepository.GetUserById(userId);
            if (userToDelete == null)
                return false;

            return _userRepository.DeleteUser(userToDelete);
        }

        public UserDto GetUserById(Guid id)
        {
            return _mapper.Map<UserDto>(_userRepository.GetUserById(id));
        }

        public IEnumerable<UserDto> GetUsersByRole(string roleName)
        {
            return _mapper.Map<IEnumerable<UserDto>>(_userRepository.GetUsersByRole(roleName));
        }

        public bool UpdateUser(UserDto user)
        {
            var userMap = _mapper.Map<User>(user);
            return _userRepository.UpdateUser(userMap);
        }

        public bool UserExists(Guid id)
        {
            return _userRepository.UserExists(id);
        }

        ICollection<UserDto> IUserService.GetAllUsers()
        {
            return _mapper.Map<ICollection<UserDto>>(_userRepository.GetAllUsers());
        }
    }
}
