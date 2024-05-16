using Reservio.Dto;
using Reservio.Helper;
using Reservio.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Reservio.Interfaces
{
    public interface IRoomService
    {
        ICollection<RoomResponseDto> GetAllRooms();
        bool RoomExists(Guid id);
        Room GetRoomById(Guid roomId);
        Task<Result> CreateRoom(RoomDto roomDto);
        bool UpadateRoom(RoomResponseDto roomDto);
        bool DeleteRoom(Guid roomId);
        Task<ICollection<RoomAvailability>> GetRoomAvailabilities(Guid roomId, DateTime date);

    }
}
