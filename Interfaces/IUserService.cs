using Reservio.Dto;
using Reservio.Helper;
using Reservio.Models;

namespace Reservio.Interfaces
{
    public interface IUserService
    {
        ICollection<UserResponseDto> GetAllUsers();
        User GetUserById(Guid id);
        IEnumerable<UserResponseDto> GetUsersByRole(string roleName);
        bool UserExists(Guid id);
        bool VerifyUser(Guid id);
        RegisterResponse RegisterUser(RegisterRequest registerRequest);
        bool UpdateUser(UpdateUserDto user);
        bool UpdateUser(User user);
        bool DeleteUser(Guid userId);
        IEnumerable<string> GetUserRoles(Guid userId);
        User GetUserByEmail(string email);
        bool UserVerified(Guid id);
        IEnumerable<NotificationResponseDto> GetAllNotifications(Guid userId);
        IEnumerable<ReservationResponseDto> GetAllReservations(Guid userId);
        Result ChangePassword(Guid userId, ChangePasswordRequestDto changePasswordRequestDto);
        bool EnableUser(Guid userId);
        bool DisableUser(Guid userId);


    }
}
