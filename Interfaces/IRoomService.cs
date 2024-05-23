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
        Task<RoomResponseDto> GetRoomResponseById(Guid roomId);
        Task<Room> GetRoomById(Guid roomId);
        Task<Result> CreateRoom(RoomDto roomDto);
        Task<Result> UpdateRoom(RoomRequestDto roomDto);
        Task<bool> DeleteRoom(Guid roomId);
        Task<ICollection<RoomAvailability>> GetRoomAvailabilities(Guid roomId, DateTime date);
        Task<Result> UpdateRoomEquipments(Guid roomId, ICollection<Guid> equipmentIds);

    }
}
