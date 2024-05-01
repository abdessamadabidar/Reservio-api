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
        [ProducesResponseType(200, Type = typeof(ICollection<RoomDto>))]
        public IActionResult GetAllRooms()
        {
            var rooms = _roomService.GetAllRooms();
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
            var rooms = _roomService.GetReservedRooms();
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
            var rooms = _roomService.GetUnReservedRooms();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(rooms);
        }


        [HttpGet("{roomId:guid}")]
        [ProducesResponseType(200, Type = typeof(RoomDto))]
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
        public IActionResult CreateRoom( [FromBody] RoomDto roomDto )
        {
            if (roomDto == null)
                return BadRequest(ModelState);

            var rooms = _roomService.GetAllRooms()
                .Where(r => r.Code == roomDto.Code)
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
        public IActionResult UpdateRoom(Guid RoomId, [FromBody] RoomDto UpdatedRoomDto)
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
                ModelState.AddModelError("", $"Something went wrong updating the room {UpdatedRoomDto.Code}");
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
