using System;
using System.Diagnostics.Eventing.Reader;
using Reservio.Dto;
using Reservio.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace Reservio.Interfaces
{
    public interface IRoomRepository
    {
        ICollection<Room> GetAllRooms();
        bool RoomExists(Guid id);
        Room GetRoomById(Guid roomId);
        Task<bool> CreateRoom(Room roomMap);
        bool UpdateRoom(Room roomMap);
        bool DeleteRoom(Room room);
        Task<ICollection<RoomAvailability>> roomAvailabilities(Guid roomId, DateTime date);
        bool Save();
    }
}
