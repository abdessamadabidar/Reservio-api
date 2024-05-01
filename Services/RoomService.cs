using AutoMapper;
using Reservio.Dto;
using Reservio.Interfaces;
using Reservio.Models;
using Reservio.Repositories;

namespace Reservio.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public RoomService(IRoomRepository roomRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public bool CreateRoom(RoomDto room)
        {
            var roomMap = _mapper.Map<Room>(room); 
            return _roomRepository.CreateRoom(roomMap);
        }

        public bool DeleteRoom(Guid roomId)
        {
            var roomToDelete = _roomRepository.GetRoomById(roomId);
            if (roomToDelete == null)
            {
                return false;
            }
            return _roomRepository.DeleteRoom(roomToDelete);
        }

        public ICollection<RoomDto> GetAllRooms()
        {
            return _mapper.Map<List<RoomDto>>(_roomRepository.GetAllRooms());
        }

        public ICollection<RoomDto> GetReservedRooms()
        {
            return _mapper.Map<List<RoomDto>>(_roomRepository.GetReservedRooms());
        }

        public RoomDto GetRoomById(Guid roomId)
        {
            return _mapper.Map<RoomDto>(_roomRepository.GetRoomById(roomId));
        }

        public ICollection<RoomDto> GetUnReservedRooms()
        {
            return _mapper.Map<List<RoomDto>>(_roomRepository.GetUnReservedRooms());
        }

        public bool RoomExists(Guid id)
        {
            return _roomRepository.RoomExists(id);
        }

        public bool UpadateRoom(RoomDto roomDto)
        {
            var roomMap = _mapper.Map<Room>(roomDto);
            return _roomRepository.UpdateRoom(roomMap);
        }
    }
}
