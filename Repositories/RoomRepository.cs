using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Reservio.Data;
using Reservio.Dto;
using Reservio.Interfaces;
using Reservio.Models;
using System.Runtime.InteropServices.JavaScript;

namespace Reservio.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;
        public RoomRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<bool> AddRoomEquipments(Guid roomId, Guid equipmentId)
        {
            _context.Add(new RoomEquipment { RoomId = roomId, EquipmentId = equipmentId });
            return Save();
        }

        public Task<bool> ClearRoomEquipments(Guid roomId)
        {
            var roomEquipments = _context.RoomEquipments.Where(re => re.RoomId == roomId);
            _context.RoomEquipments.RemoveRange(roomEquipments);
            return Save();
        }

        public async Task<bool> CreateRoom(Room roomMap)
        {
            _context.Add(roomMap);
            return await Save();
        }

        public async Task<bool> DeleteRoom(Room room)
        {
            _context.Remove(room);
            return await Save();
        }

        public ICollection<Room> GetAllRooms()
        {
            return _context
                .Rooms
                .Include(room => room.RoomEquipments)
                .ThenInclude(roomEquipment => roomEquipment.Equipment)
                .OrderByDescending(r => r.CreatedAt).ToList();
        }


        public async Task<Room> GetRoomById(Guid roomId)
        {
            return _context
                .Rooms
                .Include(room => room.Reservations)
                .Include(room => room.RoomEquipments)
                .ThenInclude(RoomEquipment => RoomEquipment.Equipment)
                .Where(r => r.Id == roomId).FirstOrDefault();
        }

        public async Task<ICollection<RoomAvailability>> roomAvailabilities(Guid roomId, DateTime date)
        {

            var availabilities = await _context
                .Reservations
                .Where(
                    reservation => reservation
                                    .StartDateTime
                                    .Date == date.Date
                                    && reservation.DeletedAt == null
                                    && reservation.RoomId == roomId
                                    && reservation.DeletedAt == null
                                    ).Select(reservation => new RoomAvailability
                                    {
                                        RoomId = reservation.RoomId,
                                        StartDateTime = reservation.StartDateTime,
                                        EndDateTime = reservation.EndDateTime
                                    }).ToListAsync();

            return availabilities;

        }

        public bool RoomExists(Guid id)
        {
            return _context.Rooms.Any(room => room.Id == id);
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UpdateRoom(Room roomMap)
        {
            _context.Update(roomMap);
            return await Save();
        }
    }
}
