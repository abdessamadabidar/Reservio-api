using Reservio.Dto;
using Reservio.Models;

namespace Reservio.Interfaces
{
    public interface IRoomService
    {
        ICollection<RoomResponseDto> GetAllRooms();
        bool RoomExists(Guid id);
        Room GetRoomById(Guid roomId);
        bool CreateRoom(RoomResponseDto roomDto);
        bool UpadateRoom(RoomResponseDto roomDto);
        bool DeleteRoom(Guid roomId);

    }
}
