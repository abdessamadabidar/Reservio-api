using System;
using Reservio.Models;
namespace Reservio.Interfaces
{
    public interface IRoomRepository
    {
        ICollection<Room> GetAllRooms();
        ICollection<Room> GetReservedRooms();
        ICollection<Room> GetUnReservedRooms();
        Room GetRoomById(Guid roomId);
        bool CreateRoom(Room roomMap);
    }
}
