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

        public RegisterResponse RegisterUser(RegisterRequest registerRequest)
        {
            registerRequest.Password = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);
            var userMap = _mapper.Map<User>(registerRequest);

            var user = _userRepository.GetUserByEmail(registerRequest.Email);
            // if user exists and is deleted, restore it
            if (user != null && user.DeletedAt != null)
            {
                user.DeletedAt = null;
                user.UpdatedAt = null;
                user.CreatedAt = DateTime.Now;
                user.VerifiedAt = null;

                if (!_userRepository.UpdateUser(user))
                    return null;
            }

            if (userMap == null || !_userRepository.CreateUser(userMap))
                return null;

            return _mapper.Map<RegisterResponse>(userMap);
        }

        public bool DeleteUser(Guid userId)
        {
            var userToDelete = _userRepository.GetUserById(userId);
            if (userToDelete == null)
                return false;

            return _userRepository.DeleteUser(userToDelete);
        }

        public User GetUserByEmail(string email)
        {
            return _userRepository.GetUserByEmail(email);
        }

        public UserDto GetUserById(Guid id)
        {
            return _mapper.Map<UserDto>(_userRepository.GetUserById(id));
        }

        public IEnumerable<string> GetUserRoles(Guid userId)
        {
            return _userRepository.GetUserRoles(userId);
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

        public bool VerifyUser(Guid id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
                return false;

            user.VerifiedAt = DateTime.Now;
            return _userRepository.UpdateUser(user);

        }

        public bool UserVerified(Guid id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
                return false;

            return user.VerifiedAt != null;
        }

        public IEnumerable<NotificationResponseDto> GetAllNotifications(Guid userId)
        {
            var notifications = _mapper.Map<List<NotificationResponseDto>>(_userRepository.GetUserNotifications(userId));
            return notifications;
        }
    }
}
