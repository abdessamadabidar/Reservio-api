using Microsoft.EntityFrameworkCore;
using Reservio.Data;

namespace Reservio.Hangfire
{
    public class RecurringJob : IRecurringJob
    {
        private readonly ApplicationDbContext _context;
        public RecurringJob(ApplicationDbContext context)
        {
            _context = context;
        }
        
            
        public void CleanupExpiredReservations()
        {
            var expiredReservations = _context.Reservations
            .Where(r => r.EndDateTime < DateTime.Now && r.DeletedAt == null);

            _context.Reservations.RemoveRange(expiredReservations);
            

            _context.SaveChanges();


            // .ExecuteUpdate(setters => setters.SetProperty(r => r.DeletedAt, DateTime.Now));
        }
    }
}
