using Reservio.Dto;
using Reservio.Models;

namespace Reservio.Interfaces
{
    public interface IUserService
    {
        ICollection<UserDto> GetAllUsers();
        UserDto GetUserById(Guid id);
        IEnumerable<UserDto> GetUsersByRole(string roleName);
        bool UserExists(Guid id);
        bool CreateUser(UserDto user);
        bool UpdateUser(UserDto user);
        bool DeleteUser(Guid userId);
     
    }
}
