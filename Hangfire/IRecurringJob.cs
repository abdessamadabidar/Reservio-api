namespace Reservio.Hangfire
{
    public interface IRecurringJob
    {
        public void CleanupExpiredReservations();
    }
}
