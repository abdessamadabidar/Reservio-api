using AutoMapper;
using Reservio.Dto;
using Reservio.Helper;
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

        public User GetUserById(Guid id)
        {
            return _userRepository.GetUserById(id);
        }

        public IEnumerable<string> GetUserRoles(Guid userId)
        {
            return _userRepository.GetUserRoles(userId);
        }

        public IEnumerable<UserResponseDto> GetUsersByRole(string roleName)
        {
            return _mapper.Map<IEnumerable<UserResponseDto>>(_userRepository.GetUsersByRole(roleName));
        }

        public bool UpdateUser(UpdateUserDto updateUserDto)
        {

            var user = _userRepository.GetUserById(updateUserDto.Id);
            if (user == null)
                return false;

            _mapper.Map(updateUserDto, user);

   
            return _userRepository.UpdateUser(user);
        }

        public bool UserExists(Guid id)
        {
            return _userRepository.UserExists(id);
        }

        ICollection<UserResponseDto> IUserService.GetAllUsers()
        {
            return _mapper.Map<ICollection<UserResponseDto>>(_userRepository.GetAllUsers());
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
            var user = _userRepository.GetUserById(userId);
            if (user == null)
                return null;

            var notifications = _mapper.Map<List<NotificationResponseDto>>(_userRepository.GetUserNotifications(userId));
            return notifications;
        }

        public bool UpdateUser(User user)
        {
            return _userRepository.UpdateUser(user);
        }

        public Result ChangePassword(Guid userId, ChangePasswordRequestDto changePasswordRequestDto)
        {
            var user = _userRepository.GetUserById(userId);
            if (user == null)
                return Result.UserNotFound;

            if (!BCrypt.Net.BCrypt.Verify(changePasswordRequestDto.OldPassword, user.Password))
                return Result.PasswordNotMatch;

            user.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordRequestDto.NewPassword);


            return _userRepository.UpdateUser(user) ? Result.Success : Result.ChangePasswordFailure;
        }

        public bool EnableUser(Guid userId)
        {
            var user = _userRepository.GetUserById(userId);
            if (user == null)
                return false;


            user.IsActivated = true;
            return _userRepository.UpdateUser(user);
        }

        public bool DisableUser(Guid userId)
        {
            var user = _userRepository.GetUserById(userId);
            if (user == null)
                return false;

            user.IsActivated = false;
            return _userRepository.UpdateUser(user);
        }

        public IEnumerable<ReservationResponseDto> GetAllReservations(Guid userId)
        {
            var user = _userRepository.GetUserById(userId);
            if (user == null)
                return null;


            var reservations = _userRepository.GetUserReservations(userId)
;

            return _mapper.Map<IEnumerable<ReservationResponseDto>>(reservations);
        }
    }
}
