using Microsoft.EntityFrameworkCore;
using Reservio.Models;

namespace Reservio.Data
{
    public class ApplicationDbContext : DbContext
    {
       public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
       {
       }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Notification> Notifications { get; set; }
         

        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {
            modelBuilder.Entity<RoleUser>()
                .HasKey(ru => new { ru.UserId, ru.RoleId });
            modelBuilder.Entity<Room>().Property(r => r.isReserved).HasDefaultValue(false);
            modelBuilder.Entity<RoleUser>()
                .HasOne(ru => ru.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ru => ru.UserId);

            modelBuilder.Entity<RoleUser>()
                .HasOne(ru => ru.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ru => ru.RoleId);


            modelBuilder.Entity<Reservation>()
                .HasKey(r => new { r.UserId, r.RoomId, r.StartDateTime });

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Room)
                .WithMany(r => r.Reservations)
                .HasForeignKey(r => r.RoomId);
        }
        

    }
}
