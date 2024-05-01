using Reservio.Dto;
using Reservio.Models;

namespace Reservio.Interfaces
{
    public interface IRoomService
    {
        ICollection<RoomDto> GetAllRooms();
        ICollection<RoomDto> GetReservedRooms();
        ICollection<RoomDto> GetUnReservedRooms();
        bool RoomExists(Guid id);
        RoomDto GetRoomById(Guid roomId);
        bool CreateRoom(RoomDto roomDto);
        bool UpadateRoom(RoomDto roomDto);
        bool DeleteRoom(Guid roomId);

    }
}
