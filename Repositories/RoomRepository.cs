using Microsoft.EntityFrameworkCore.Diagnostics;
using Reservio.Data;
using Reservio.Interfaces;
using Reservio.Models;

namespace Reservio.Repositories {
    public class RoomRepository : IRoomRepository {
        private readonly ApplicationDbContext _context;
        public RoomRepository(ApplicationDbContext context) { 
            _context= context;
        }

        public bool CreateRoom(Room roomMap)
        {
            _context.Add(roomMap);
            return _context.SaveChanges() > 0 ? true : false;
        }

        public ICollection<Room> GetAllRooms()
        {
            return _context.Rooms.OrderBy(r => r.Id).ToList();    
        }

        public ICollection<Room> GetReservedRooms()
        {
            return _context.Rooms.Where(r => r.isReserved == true).ToList();
        }

        public Room GetRoomById(Guid roomId)
        {
            return _context.Rooms.Where(r => r.Id == roomId).FirstOrDefault();
        }

        public ICollection<Room> GetUnReservedRooms()
        {
            return _context.Rooms.Where(r => r.isReserved == false).ToList();
        }
    }
}
