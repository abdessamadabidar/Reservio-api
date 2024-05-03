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
        public DbSet<RoleUser> RoleUsers { get; set; }
        public DbSet<Notification> Notifications { get; set; }
         

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleUser>()
                .HasKey(ru => new { ru.UserId, ru.RoleId });

            modelBuilder.Entity<RoleUser>()
                .HasOne(ru => ru.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ru => ru.UserId);

            modelBuilder.Entity<RoleUser>()
                .HasOne(ru => ru.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ru => ru.RoleId);


            modelBuilder.Entity<Room>().Property(r => r.isReserved).HasDefaultValue(false);
            
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


            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Room>()
                .HasIndex(r => r.Code)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(User => User.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<User>()
                .HasQueryFilter(user => user.DeletedAt == null);

        }


        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is BaseEntity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            entry.CurrentValues["UpdatedAt"] = DateTime.Now;
                            break;
                        case EntityState.Deleted:
                            entry.State = EntityState.Modified;
                            entry.CurrentValues["DeletedAt"] = DateTime.Now;
                            break;
                    }
                }
            }


            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Notification && entry.State == EntityState.Added)
                {
                    entry.CurrentValues["CreatedAt"] = DateTime.Now;
                }
            }


            var recentlyAddedUsers = ChangeTracker
                .Entries<User>()
                .Where(e => e.State == EntityState.Added)
                .Select(e => e.Entity);

            foreach (var user in recentlyAddedUsers)
            {
                var role = Roles?.FirstOrDefault(r => r.Name == "USER");
                if (role != null)
                {
                    user.UserRoles.Add(new RoleUser { RoleId = role.Id, UserId = user.Id,  User = user, Role = role });
                }
            }

            return base.SaveChanges();
        }






    }
}
