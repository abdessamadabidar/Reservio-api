﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Reservio.Data;
using Reservio.Interfaces;
using Reservio.Models;

namespace Reservio.Repositories {
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;
        public RoomRepository(ApplicationDbContext context) { 
            _context= context;
        }

        public bool CreateRoom(Room roomMap)
        {
            _context.Add(roomMap);
            return Save();
        }

        public bool DeleteRoom(Room roomMap)
        {
            _context.Remove(roomMap);
            return Save();
        }

        public ICollection<Room> GetAllRooms()
        {
            return _context.Rooms.OrderBy(r => r.Id).ToList();    
        }


        public Room GetRoomById(Guid roomId)
        {
            return _context.Rooms.Include(room => room.Reservations).Where(r => r.Id == roomId).FirstOrDefault();
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
