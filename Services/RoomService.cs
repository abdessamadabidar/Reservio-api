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

        public bool CreateRoom(RoomResponseDto room)
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

        public ICollection<RoomResponseDto> GetAllRooms()
        {
            return _mapper.Map<List<RoomResponseDto>>(_roomRepository.GetAllRooms());
        }
        public Room GetRoomById(Guid roomId)
        {
            return _roomRepository.GetRoomById(roomId);
        }
        public bool RoomExists(Guid id)
        {
            return _roomRepository.RoomExists(id);
        }

        public bool UpadateRoom(RoomResponseDto roomDto)
        {
            var roomMap = _mapper.Map<Room>(roomDto);
            return _roomRepository.UpdateRoom(roomMap);
        }
    }
}
