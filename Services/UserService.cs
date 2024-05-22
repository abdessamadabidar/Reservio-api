using AutoMapper;
using Microsoft.Identity.Client;
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

        public async Task<RegisterResponse> RegisterUser(RegisterRequest registerRequest)
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

                var rs = await _userRepository.UpdateUser(user);

                if (!rs)
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

        public async Task<bool> UpdateUser(UpdateUserDto updateUserDto)
        {

            var user = _userRepository.GetUserById(updateUserDto.Id);
            if (user == null)
                return false;

            _mapper.Map(updateUserDto, user);


            return await _userRepository.UpdateUser(user);
        }

        public bool UserExists(Guid id)
        {
            return _userRepository.UserExists(id);
        }

        ICollection<UserResponseDto> IUserService.GetAllUsers()
        {
            return _mapper.Map<ICollection<UserResponseDto>>(_userRepository.GetAllUsers());
        }

        public async Task<bool> VerifyUser(Guid id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
                return false;

            user.VerifiedAt = DateTime.Now;
            return await _userRepository.UpdateUser(user);

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

        public async Task<bool> UpdateUser(User user)
        {
            return await _userRepository.UpdateUser(user);
        }

        public async Task<Result> ChangePassword(Guid userId, ChangePasswordRequestDto changePasswordRequestDto)
        {
            var user = _userRepository.GetUserById(userId);
            if (user == null)
                return Result.UserNotFound;

            if (!BCrypt.Net.BCrypt.Verify(changePasswordRequestDto.OldPassword, user.Password))
                return Result.PasswordNotMatch;

            user.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordRequestDto.NewPassword);


            return await _userRepository.UpdateUser(user) ? Result.Success : Result.ChangePasswordFailure;
        }

        public async Task<bool> EnableUser(Guid userId)
        {
            var user = _userRepository.GetUserById(userId);
            if (user == null)
                return false;


            user.IsActivated = true;
            return await _userRepository.UpdateUser(user);
        }

        public async Task<bool> DisableUser(Guid userId)
        {
            var user = _userRepository.GetUserById(userId);
            if (user == null)
                return false;

            user.IsActivated = false;
            return await _userRepository.UpdateUser(user);
        }

        public async Task<bool> ApproveUser(Guid userId)
        {
            var user = _userRepository.GetUserById(userId);
            if (user == null)
                return false;

            user.IsApproved = true;
            return await _userRepository.UpdateUser(user);
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

        public async Task<List<Guid>> GetAdmins()
        {
            return await _userRepository.GetAdmins();
        }
    }
}
