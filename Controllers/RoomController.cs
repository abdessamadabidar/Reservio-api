using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservio.Data;
using Reservio.Dto;
using Reservio.Interfaces;
using Reservio.Models;
using Reservio.Services;

namespace Reservio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }


        [HttpGet("all")]
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
        public IActionResult CreateRoom( [FromBody] RoomResponseDto roomDto )
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


            if (!_roomService.CreateRoom(roomDto))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
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

        [HttpDelete("{RoomId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUser(Guid roomId)
        {
            if (!_roomService.RoomExists(roomId))
            {
                return NotFound();
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_roomService.DeleteRoom(roomId))
            {
                ModelState.AddModelError("", "Something went wrong deleting the room");
                return StatusCode(500, ModelState);
            }

            return Ok("Room deleted successfully");
        }



    }
}
