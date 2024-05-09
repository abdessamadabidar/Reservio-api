using Microsoft.EntityFrameworkCore;
using Reservio.Data;
using Reservio.Interfaces;
using Reservio.Models;

namespace Reservio.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateReservationAsync(Reservation reservation)
        {
            using(var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    
                    await _context.Reservations.AddAsync(reservation);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
            
        }

        public bool DeleteReservation(Reservation reservation)
        {
            _context.Reservations.Remove(reservation);
            return Save();
        }

        public IEnumerable<Reservation> GetAllReservations()
        {
            return _context.Reservations
                .Include(reservation => reservation.User)
                .Include(reservation => reservation.Room)
                .OrderByDescending(reservation => reservation.CreatedAt)
                .ToList();
        }

        public Reservation GetReservationById(Guid Id)
        {
            return _context.Reservations
                .Include(reservation => reservation.User)
                .Include(reservation => reservation.Room)
                .FirstOrDefault(reservation => reservation.Id == Id);

        }
        
        public bool ReservationExists(Guid Id)
        {
            return _context.Reservations.Any(reservation => reservation.Id == Id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateReservation(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            return Save();

        }
    }
}
