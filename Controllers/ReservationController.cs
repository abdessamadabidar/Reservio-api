using Microsoft.AspNetCore.Mvc;
using Reservio.Dto;
using Reservio.Interfaces;

namespace Reservio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IRoomService _roomService;
        private readonly IUserService _userService;
        public ReservationController(IReservationService reservationService, IRoomService roomService, IUserService userService)
        {
            _reservationService = reservationService;
            _roomService = roomService;
            _userService = userService;
        }


        [HttpGet]
        [ProducesResponseType(200)]

        public IActionResult GetAllReservations()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var reservations = _reservationService.GetAllReservations();
            return Ok(reservations);
        }

        [HttpGet("{Id:guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetReservationById(Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!_reservationService.ReservationExists(Id))
            {
                return NotFound("Reservation does not exist");
            }

            var reservation = _reservationService.GetReservationById(Id);
            return Ok(reservation);
        }


        [HttpPost("new")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddReservation([FromBody] ReservationRequestDto reservationRequestDto)
        {
            if (!ModelState.IsValid || reservationRequestDto == null)
            {
                return BadRequest();
            }

            if (!_roomService.RoomExists(reservationRequestDto.RoomId))
            {
                return BadRequest("Room does not exist");
            }

            if (!_userService.UserExists(reservationRequestDto.UserId))
            {
                return BadRequest("User does not exist");
            }

            if (reservationRequestDto.StartDateTime >= reservationRequestDto.EndDateTime)
            {
                ModelState.AddModelError("Reservation", "End date must be greater than start date");
                return StatusCode(400, ModelState);
            }

            if(reservationRequestDto.StartDateTime < DateTime.UtcNow || reservationRequestDto.EndDateTime < DateTime.UtcNow)
            {
                ModelState.AddModelError("Reservation", "Invalid Date time");
                return StatusCode(400, ModelState);
            }

            
            var user = _userService.GetUserById(reservationRequestDto.UserId);
            if (!user.IsApproved)
            {
                ModelState.AddModelError("User", "User is not approved");
                return StatusCode(400, ModelState);
            }


            if (!user.IsActivated)
            {
                ModelState.AddModelError("User", "User is not activated");
                return StatusCode(400, ModelState);
            }


            var result = await _reservationService.CreateReservationAsync(reservationRequestDto);
            if (!result)
            {
                 ModelState.AddModelError("Reservation", "Reservation already registered");
                return StatusCode(500, ModelState);
            }

             
            return Ok("Reservation added successfully");
        }


        [HttpPut("{reservationId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReservation(Guid reservationId, [FromBody] ReservationRequestDto UpdatedReservationDto)
        {
            if (UpdatedReservationDto == null || reservationId != UpdatedReservationDto.Id)
            {
                return BadRequest(ModelState);
            }


            if (!_roomService.RoomExists(UpdatedReservationDto.RoomId))
            {
                return BadRequest("Room does not exist");
            }

            if (!_userService.UserExists(UpdatedReservationDto.UserId))
            {
                return BadRequest("User does not exist");
            }

            if (!_reservationService.ReservationExists(reservationId))
            {
                return NotFound("reservation does not exist");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (UpdatedReservationDto.StartDateTime >= UpdatedReservationDto.EndDateTime)
            {
                ModelState.AddModelError("Reservation", "End date must be greater than start date");
                return StatusCode(400, ModelState);
            }

            if (!_reservationService.UpdateReservation(UpdatedReservationDto))
            {
                ModelState.AddModelError("", $"Something went wrong updating the reservation {UpdatedReservationDto.Id}");
                return StatusCode(500, ModelState);
            }

            return Ok("Reservation updated successfully");
        }

        [HttpDelete("{reservationId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReservation(Guid reservationId)
        {
            if (!_reservationService.ReservationExists(reservationId))
            {
                return NotFound("Reservation does not exist");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_reservationService.DeleteReservation(reservationId))
            {
                ModelState.AddModelError("", "Something went wrong deleting the reservation");
                return StatusCode(500, ModelState);
            }

            return Ok("Reservation deleted successfully");
        }
    }
}
