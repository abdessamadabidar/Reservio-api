using Microsoft.EntityFrameworkCore;
using Reservio.Data;
using Reservio.Interfaces;
using Reservio.Models;

namespace Reservio.Repositories
{
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly ApplicationDbContext _context;

        public EquipmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddEquipment(Equipment equipment)
        {
             _context.Equipments.Add(equipment);
            return Save();
        }

        public async Task<bool> EquipmentExists(Guid id)
        {
            return await _context.Equipments.AnyAsync(equipment => equipment.Id == id);
        }

        public async Task<ICollection<Equipment>> GetAllEquipments()
        {
            return _context.Equipments.ToList();
        }

        public async Task<Equipment> GetEquipmentById(Guid id)
        {
            return await _context.Equipments.FindAsync(id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}
