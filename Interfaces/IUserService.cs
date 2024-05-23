using Reservio.Dto;
using Reservio.Helper;
using Reservio.Models;

namespace Reservio.Interfaces
{
    public interface IUserService
    {
        ICollection<UserResponseDto> GetAllUsers();
        Task<IEnumerable<UserResponseDto>> GetRecentUsers();
        User GetUserById(Guid id);
        IEnumerable<UserResponseDto> GetUsersByRole(string roleName);
        bool UserExists(Guid id);
        Task<bool> VerifyUser(Guid id);
        Task<RegisterResponse> RegisterUser(RegisterRequest registerRequest);
        Task<bool> UpdateUser(UpdateUserDto user);
        Task<bool> UpdateUser(User user);
        bool DeleteUser(Guid userId);
        IEnumerable<string> GetUserRoles(Guid userId);
        User GetUserByEmail(string email);
        bool UserVerified(Guid id);
        IEnumerable<NotificationResponseDto> GetAllNotifications(Guid userId);
        IEnumerable<ReservationResponseDto> GetAllReservations(Guid userId);
        Task<Result> ChangePassword(Guid userId, ChangePasswordRequestDto changePasswordRequestDto);
        Task<bool> EnableUser(Guid userId);
        Task<bool> DisableUser(Guid userId);
        Task<bool> ApproveUser(Guid userId);
        Task<List<Guid>> GetAdmins();


    }
}
