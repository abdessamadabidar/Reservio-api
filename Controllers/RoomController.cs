using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservio.Data;
using Reservio.Dto;
using Reservio.Helper;
using Reservio.Interfaces;
using Reservio.Models;
using Reservio.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Reservio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        

        public RoomController(IRoomService roomService, IWebHostEnvironment environment)
        {
            _roomService = roomService;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<RoomResponseDto>))]
        public IActionResult GetAllRooms()
        {
            var rooms = _roomService.GetAllRooms();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(rooms);
        }


        [HttpGet("{roomId:guid}")]
        [ProducesResponseType(200, Type = typeof(RoomResponseDto))]
        public IActionResult GetRoomById(Guid roomId)
        {
            var room = _roomService.GetRoomById(roomId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(room);
        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateRoom( [FromForm] RoomDto roomDto )
        {
            if (roomDto == null)
                return BadRequest(ModelState);

            var rooms = _roomService.GetAllRooms()
                .Where(r => r.Name == roomDto.Name)
                .FirstOrDefault();

            if (rooms != null)
            {
                ModelState.AddModelError("", "room already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            
            var result = await _roomService.CreateRoom(roomDto);


            if (result == Result.CreateRoomFailure)
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            if (result == Result.UploadImageFailure)
            {
                ModelState.AddModelError("", "Something went wrong while uploading the image");
                return StatusCode(500, ModelState);
            }

            return Ok("Room successfully created");
        }

        [HttpPut("{RoomId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateRoom(Guid RoomId, [FromBody] RoomResponseDto UpdatedRoomDto)
        {
            if (UpdatedRoomDto == null || RoomId != UpdatedRoomDto.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_roomService.RoomExists(RoomId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }



            if (!_roomService.UpadateRoom(UpdatedRoomDto))
            {
                ModelState.AddModelError("", $"Something went wrong updating the room {UpdatedRoomDto.Name}");
                return StatusCode(500, ModelState);
            }

            return Ok("Room updated successfully");
        }



        [HttpDelete("{roomId:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteRoom(Guid roomId)
        {
            if (!_roomService.RoomExists(roomId))
            {
                return NotFound("Room not found");
            }


            var room = _roomService.GetRoomById(roomId);
            if (room.DeletedAt != null)
            {
                ModelState.AddModelError("", $"Room not found");
                return StatusCode(404, ModelState);
            }


            if (!_roomService.DeleteRoom(roomId))
            {
                ModelState.AddModelError("", $"Something went wrong deleting the room with id {roomId}");
                return StatusCode(500, ModelState);
            }

            return Ok("Room deleted successfully");
        }


        [HttpGet("{roomId:guid}/availabilities")]
        [ProducesResponseType(200, Type = typeof(ICollection<RoomAvailability>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetRoomAvailability(Guid roomId, [FromQuery] DateTime date)
        {
            // The return type of this method is Task<IActionResult> but the return type of the method in the IRoomService interface is Task<ICollection<RoomAvailability>>
            if (!_roomService.RoomExists(roomId))
            {
                return NotFound("Room not found");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            return Ok(await _roomService.GetRoomAvailabilities(roomId, date));
        }

    }
}
