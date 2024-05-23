using AutoMapper;
using Reservio.Dto;
using Reservio.Helper;
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
        private readonly INotificationService _notificationService;

        public ReservationService(IReservationRepository reservationRepository, IMapper mapper, IRoomService roomService, IUserService userService, INotificationService notificationService)
        {
            _reservationRepository = reservationRepository;
            _mapper = mapper;
            _roomService = roomService;
            _userService = userService;
            _notificationService = notificationService;
        }

        public async Task<Result> CreateReservationAsync(ReservationRequestDto reservationRequestDto)
        {


            if (!await IsRoomAvailable(reservationRequestDto.RoomId, reservationRequestDto.StartDateTime, reservationRequestDto.EndDateTime))
            {
                return Result.RoomNotAvailable;
            }


            if (!await UserCanReserve(reservationRequestDto.UserId, reservationRequestDto.StartDateTime))
            {
                return Result.UserCannotReserve;
            }

            try
            {
                var reservation = _mapper.Map<Reservation>(reservationRequestDto);
                var res = await _reservationRepository.CreateReservationAsync(reservation);
                if (!res)
                {
                    return Result.ReservationFailure;
                }


                var user = _userService.GetUserById(reservationRequestDto.UserId);
                var room = await _roomService.GetRoomById(reservationRequestDto.RoomId);

                // send notification to admin
                var reveivers = await _userService.GetAdmins();
                reveivers.Append(reservationRequestDto.UserId);
                string title = "New Reservation";
                string body = $"The user {user.FirstName} {user.LastName} has reserved the room with name {room.Name} at the day {reservationRequestDto.StartDateTime.ToShortDateString()} from {reservationRequestDto.StartDateTime.ToShortTimeString()} to {reservationRequestDto.EndDateTime.ToShortTimeString()}";

                _notificationService.Notifiy(reveivers, title, body);

                return Result.Success;
            }

            catch (Exception ex)
            {
                return Result.ReservationFailure;

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

        public async Task<ICollection<ReservationResponseDto>> GetAllReservations()
        {
            var reservations = await _reservationRepository.GetAllReservations();
            var reservationsDto = _mapper.Map<ICollection<ReservationResponseDto>>(reservations);
            return reservationsDto;
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

        public async Task<bool> UpdateReservation(ReservationRequestDto reservationRequestDto)
        {
            if (!await IsRoomAvailable(reservationRequestDto.RoomId, reservationRequestDto.StartDateTime, reservationRequestDto.EndDateTime))
            {
                return false;
            }


            var reservation = _mapper.Map<Reservation>(reservationRequestDto);
            return _reservationRepository.UpdateReservation(reservation);
        }






        private async Task<bool> IsRoomAvailable(Guid roomId, DateTime startDateTime, DateTime endDateTime)
        {

            // Check if the room is available for the requested time

            var reservations = await _reservationRepository.GetAllReservations();

            bool IsNotAvailable = reservations.Any(
                    r => r.StartDateTime == startDateTime
                    && r.EndDateTime == endDateTime
                    && r.DeletedAt == null
                    && r.RoomId == roomId
                );

            if (IsNotAvailable)
            {
                return false;
            }

            return true;
        }


        private async Task<bool> UserCanReserve(Guid userId, DateTime startDateTime)
        {
            // Check if the user has a reservation on the same day
            var reservations = await _reservationRepository.GetAllReservations();
            bool hasReservation = reservations.Any(
                r => r.UserId == userId
                && r.DeletedAt == null
                && r.StartDateTime.Date == startDateTime.Date
            );

            return !hasReservation;
        }
    }
}
