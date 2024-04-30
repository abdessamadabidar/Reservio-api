using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservio.Data;
using Reservio.Dto;
using Reservio.Interfaces;
using Reservio.Models;

namespace Reservio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : Controller
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public RoomsController(IRoomRepository roomRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }
        [HttpGet("all")]
        [ProducesResponseType(200, Type = typeof(ICollection<RoomDto>))]
        public IActionResult GetAllRooms()
        {
            var rooms = _mapper.Map<List<RoomDto>>(_roomRepository.GetAllRooms());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(rooms);
        }
        [HttpGet("reserved")]
        [ProducesResponseType(200, Type = typeof(ICollection<RoomDto>))]
        public IActionResult GetReservedRooms()
        {
            var rooms = _mapper.Map<List<RoomDto>>(_roomRepository.GetReservedRooms());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(rooms);
        }
        [HttpGet("unreserved")]
        [ProducesResponseType(200, Type = typeof(ICollection<RoomDto>))]
        public IActionResult GetUnReservedRooms()
        {
            var rooms = _mapper.Map<List<RoomDto>>(_roomRepository.GetUnReservedRooms());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(rooms);
        }
        [HttpGet("Room/{roomId}")]
        [ProducesResponseType(200, Type = typeof(RoomDto))]
        public IActionResult GetRoomById(int roomId)
        {
            var room = _mapper.Map<RoomDto>(_roomRepository.GetRoomById(roomId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(room);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateRoom( [FromBody] String roomCode )
        {
            if (roomCode == null)
                return BadRequest(ModelState);

            var rooms = _roomRepository.GetAllRooms()
                .Where(r => r.Code == roomCode)
                .FirstOrDefault();

            if (rooms != null)
            {
                ModelState.AddModelError("", "room already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Room room = new Room(roomCode);


            if (!_roomRepository.CreateRoom(room))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }


    }
}
