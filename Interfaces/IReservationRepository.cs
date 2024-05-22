
using Reservio.Models;

namespace Reservio.Interfaces
{
    public interface IReservationRepository
    {
        public Task<bool> CreateReservationAsync(Reservation reservation);
        public Task<IEnumerable<Reservation>> GetAllReservations();
        public Reservation GetReservationById(Guid Id);
        public bool ReservationExists(Guid Id);
        public bool UpdateReservation(Reservation reservation);
        public bool DeleteReservation(Reservation reservation);
        public bool Save();
    }
}
