using AutoMapper;
using Reservio.Dto;
using Reservio.Interfaces;
using Reservio.Models;
using System.Drawing.Printing;

namespace Reservio.Services
{
    public class EquipmentService : IEquipmentService
    {

        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IMapper _mapper;

        public EquipmentService(IEquipmentRepository equipmentRepository, IMapper mapper)
        {
            _equipmentRepository = equipmentRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddEquipment(EquipmentDto equipmentDto)
        {
            var equipment = _mapper.Map<Equipment>(equipmentDto);
            return await _equipmentRepository.AddEquipment(equipment);
        }

        public async Task<bool> EquipmentExists(Guid id)
        {
            return await _equipmentRepository.EquipmentExists(id);
        }

        public async Task<ICollection<EquipmentDto>> GetAllEquipments()
        {
            var equipments = _mapper.Map<ICollection<EquipmentDto>>(await _equipmentRepository.GetAllEquipments());
            return equipments;
        }

        public async Task<EquipmentDto> GetEquipmentById(Guid id)
        {
            var equipment = _mapper.Map<EquipmentDto>(await _equipmentRepository.GetEquipmentById(id));
            return equipment;
        }
    }
}
