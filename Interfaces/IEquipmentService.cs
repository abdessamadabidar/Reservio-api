

using Reservio.Dto;
using Reservio.Models;

namespace Reservio.Interfaces
{
    public interface IEquipmentService
    {
        public Task<bool> AddEquipment(EquipmentDto equipmentDto);
        public Task<ICollection<EquipmentDto>> GetAllEquipments();
        public Task<bool> EquipmentExists(Guid id);
        public Task<EquipmentDto> GetEquipmentById(Guid id);
    }
}
