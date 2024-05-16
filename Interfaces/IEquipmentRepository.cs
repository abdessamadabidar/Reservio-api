using Reservio.Models;

namespace Reservio.Interfaces
{
    public interface IEquipmentRepository
    {
        public Task<bool> AddEquipment(Equipment equipment);
        public Task<ICollection<Equipment>> GetAllEquipments();
        public Task<bool> EquipmentExists(Guid id);
        public Task<Equipment> GetEquipmentById(Guid id);
        public bool Save();
    }
}
