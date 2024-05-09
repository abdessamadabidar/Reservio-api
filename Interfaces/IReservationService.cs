using Reservio.Dto;
using Reservio.Models;
using System.Collections.ObjectModel;

namespace Reservio.Interfaces
{
    public interface IReservationService
    {
        public Task<bool> CreateReservationAsync(ReservationRequestDto reservationRequestDto);
        public IEnumerable<ReservationResponseDto> GetAllReservations();
        public ReservationResponseDto GetReservationById(Guid Id);
        public bool ReservationExists(Guid Id);
        public bool UpdateReservation(ReservationRequestDto reservationRequestDto);
        public bool DeleteReservation(Guid Id);
    }
}
