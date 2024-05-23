using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;


        public RoomController(IRoomService roomService, IWebHostEnvironment environment)
        {
            _roomService = roomService;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<RoomResponseDto>))]
        [Authorize(Roles = "USER")]
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
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> GetRoomById(Guid roomId)
        {
            var room = await _roomService.GetRoomResponseById(roomId);
            if (room == null)
            {
                return NotFound("Room not found");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(room);
        }


        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateRoom([FromForm] RoomDto roomDto)
        {
            if (roomDto == null)
                return BadRequest(ModelState);

            Console.WriteLine("==============================================================");
            Console.WriteLine(roomDto.ToJson());

            var rooms = _roomService.GetAllRooms()
                .Where(r => r.Name == roomDto.Name)
                .FirstOrDefault();

            if (rooms != null)
            {
                ModelState.AddModelError("Room", "room already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);



            var result = await _roomService.CreateRoom(roomDto);


            if (result == Result.CreateRoomFailure)
            {
                ModelState.AddModelError("Room", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            if (result == Result.UploadImageFailure)
            {
                ModelState.AddModelError("Room", "Something went wrong while uploading the image");
                return StatusCode(500, ModelState);
            }

            return Ok("Room successfully created");
        }

        [HttpPut("{RoomId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateRoom(Guid RoomId, [FromForm] RoomRequestDto UpdatedRoomDto)
        {
            if (UpdatedRoomDto == null)
            {
                return BadRequest(ModelState);
            }

            if (RoomId != UpdatedRoomDto.Id)
            {
                ModelState.AddModelError("Room", "Room id mismatch");
                return StatusCode(422, ModelState);
            }

            if (!_roomService.RoomExists(RoomId))
            {
                return NotFound("Room not found");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _roomService.UpdateRoom(UpdatedRoomDto);

            if (result == Result.UploadImageFailure)
            {
                ModelState.AddModelError("Room", "Something went wrong while uploading the image");
                return StatusCode(500, ModelState);
            }

            if (result == Result.UpdateRoomFailure)
            {
                ModelState.AddModelError("Room", $"Something went wrong updating the room {UpdatedRoomDto.Name}");
                return StatusCode(500, ModelState);
            }



            return Ok("Room updated successfully");
        }



        [HttpDelete("{roomId:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteRoom(Guid roomId)
        {
            if (!_roomService.RoomExists(roomId))
            {
                return NotFound("Room not found");
            }


            var room = await _roomService.GetRoomById(roomId);
            if (room.DeletedAt != null)
            {
                ModelState.AddModelError("Room", $"Room not found");
                return StatusCode(404, ModelState);
            }


            if (!await _roomService.DeleteRoom(roomId))
            {
                ModelState.AddModelError("Room", $"Something went wrong deleting the room with id {roomId}");
                return StatusCode(500, ModelState);
            }

            return Ok("Room deleted successfully");
        }


        [HttpGet("{roomId:guid}/availabilities")]
        [ProducesResponseType(200, Type = typeof(ICollection<RoomAvailability>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "USER")]
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


        [HttpPut("{roomId:guid}/equipments")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "USER")]
        public async Task<IActionResult> UpdateRoomEquipments(Guid roomId, [FromBody] ICollection<Guid> equipmentIds)
        {
            if (!_roomService.RoomExists(roomId))
            {
                return NotFound("Room not found");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _roomService.UpdateRoomEquipments(roomId, equipmentIds);

            if (result == Result.UpdateRoomFailure)
            {
                ModelState.AddModelError("", "Something went wrong while updating the room equipments");
                return StatusCode(500, ModelState);
            }

            return Ok("Room equipments updated successfully");
        }
    }
}
