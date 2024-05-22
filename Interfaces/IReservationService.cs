using Reservio.Dto;
using Reservio.Helper;
using Reservio.Models;
using System.Collections.ObjectModel;

namespace Reservio.Interfaces
{
    public interface IReservationService
    {
        public Task<Result> CreateReservationAsync(ReservationRequestDto reservationRequestDto);
        public Task<ICollection<ReservationResponseDto>> GetAllReservations();
        public ReservationResponseDto GetReservationById(Guid Id);
        public bool ReservationExists(Guid Id);
        public Task<bool> UpdateReservation(ReservationRequestDto reservationRequestDto);
        public bool DeleteReservation(Guid Id);
    }
}
