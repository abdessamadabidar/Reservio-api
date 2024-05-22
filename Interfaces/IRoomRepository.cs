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
        Task<Room> GetRoomById(Guid roomId);
        Task<bool> CreateRoom(Room roomMap);
        Task<bool> UpdateRoom(Room roomMap);
        Task<bool> DeleteRoom(Room room);
        Task<ICollection<RoomAvailability>> roomAvailabilities(Guid roomId, DateTime date);
        Task<bool> ClearRoomEquipments(Guid roomId);
        Task<bool> AddRoomEquipments(Guid roomId, Guid equipmentId);

        Task<bool> Save();
    }
}
