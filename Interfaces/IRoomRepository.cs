using System;
using Reservio.Models;
namespace Reservio.Interfaces
{
    public interface IRoomRepository
    {
        ICollection<Room> GetAllRooms();
        bool RoomExists(Guid id);
        Room GetRoomById(Guid roomId);
        bool CreateRoom(Room roomMap);
        bool UpdateRoom(Room roomMap);
        bool DeleteRoom(Room roomMap);
        bool Save();
    }
}
