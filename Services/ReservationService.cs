using AutoMapper;
using Reservio.Dto;
using Reservio.Interfaces;
using Reservio.Models;
using System.Collections.ObjectModel;

namespace Reservio.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IMapper _mapper;
        private readonly IReservationRepository _reservationRepository;
        private readonly IRoomService _roomService;
        private readonly IUserService _userService;

        public ReservationService(IReservationRepository reservationRepository,  IMapper mapper, IRoomService roomService, IUserService userService)
        {
            _reservationRepository = reservationRepository;
            _mapper = mapper;
            _roomService = roomService;
            _userService = userService;
        }

        public async Task<bool> CreateReservationAsync(ReservationRequestDto reservationRequestDto)
        {


            if (!IsRoomAvailable(reservationRequestDto.RoomId, reservationRequestDto.StartDateTime, reservationRequestDto.EndDateTime))
            {
                return false;
            }


            if (!UserCanReserve(reservationRequestDto.UserId))
            {
                return false;
            }

            try
            {
                var reservation = _mapper.Map<Reservation>(reservationRequestDto);
                return await _reservationRepository.CreateReservationAsync(reservation);
            }

            catch (Exception ex)
            {
                return false;
            }
            
        }


        public bool DeleteReservation(Guid Id)
        {
            var reservation = _reservationRepository.GetReservationById(Id);
            if (reservation == null)
            {
                return false;
            }

            return _reservationRepository.DeleteReservation(reservation);
        }

        public IEnumerable<ReservationResponseDto> GetAllReservations()
        {
            var reservations = _mapper.Map<IEnumerable<ReservationResponseDto>>(_reservationRepository.GetAllReservations());
            return reservations;
        }

        public ReservationResponseDto GetReservationById(Guid Id)
        {
            var reservation = _mapper.Map<ReservationResponseDto>(_reservationRepository.GetReservationById(Id));
            return reservation;
        }

        public bool ReservationExists(Guid Id)
        {
            return _reservationRepository.ReservationExists(Id);
        }

        public bool UpdateReservation(ReservationRequestDto reservationRequestDto)
        {
            if(!IsRoomAvailable(reservationRequestDto.RoomId, reservationRequestDto.StartDateTime, reservationRequestDto.EndDateTime))
            {
                return false;
            }

            if (!UserCanReserve(reservationRequestDto.UserId))
            {
                return false;
            }

            var reservation = _mapper.Map<Reservation>(reservationRequestDto);
            return _reservationRepository.UpdateReservation(reservation);
        }






        private bool IsRoomAvailable(Guid roomId, DateTime startDateTime, DateTime endDateTime)
        {
            var room = _roomService.GetRoomById(roomId);

            // Check if the room is available for the requested time
            if (room.Reservations.Any(r => r.StartDateTime == startDateTime && r.EndDateTime == endDateTime && r.DeletedAt == null))
            {
                return false;
            }

            return true;
        }


        private bool UserCanReserve(Guid userId)
        {
            var StartOfDay = DateTime.Now.Date;
            var EndOfDay = StartOfDay.AddDays(1).AddTicks(-1);

            var user = _userService.GetUserById(userId);

            // Check if the user has a reservation for the requested time
            if (user.Reservations.Any(r => r.StartDateTime >= StartOfDay && r.StartDateTime <= EndOfDay && r.DeletedAt == null))
            {
                return false;
            }

            return true;
        }
    }
}
