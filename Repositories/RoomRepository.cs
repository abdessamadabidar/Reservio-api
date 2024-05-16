using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Reservio.Data;
using Reservio.Dto;
using Reservio.Interfaces;
using Reservio.Models;
using System.Runtime.InteropServices.JavaScript;

namespace Reservio.Repositories {
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;
        public RoomRepository(ApplicationDbContext context) { 
            _context= context;
        }

        public async Task<bool> CreateRoom(Room roomMap)
        {
             _context.Add(roomMap);
            return Save();
        }

        public bool DeleteRoom(Room room)
        {
            _context.Remove(room);
            return Save();
        }

        public ICollection<Room> GetAllRooms()
        {
            return _context
                .Rooms
                .Include(room => room.RoomEquipments)
                .ThenInclude(roomEquipment => roomEquipment.Equipment)
                .OrderByDescending(r => r.CreatedAt).ToList();    
        }


        public Room GetRoomById(Guid roomId)
        {
            return _context.Rooms.Include(room => room.Reservations).Where(r => r.Id == roomId).FirstOrDefault();
        }

        public async Task<ICollection<RoomAvailability>> roomAvailabilities(Guid roomId, DateTime date)
        {
           
            var availabilities = await _context
                .Reservations
                .Where(
                    reservation => reservation
                                    .StartDateTime
                                    .Date == date.Date && reservation.DeletedAt == null
                                    ).Select(reservation => new RoomAvailability
                                    {
                                        RoomId = reservation.RoomId,
                                        StartDateTime = reservation.StartDateTime,
                                        EndDateTime = reservation.EndDateTime
                                    }).ToListAsync();

            return  availabilities;

        }

        public bool RoomExists(Guid id)
        {
            return _context.Rooms.Any(room => room.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateRoom(Room roomMap)
        {
            _context.Update(roomMap);
            return Save();
        }
    }
}
