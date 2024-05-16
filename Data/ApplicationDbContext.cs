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
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<RoomEquipment> RoomEquipments { get; set; }
         

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


            modelBuilder.Entity<RoomEquipment>()
           .HasKey(re => new { re.RoomId, re.EquipmentId });

            modelBuilder.Entity<RoomEquipment>()
                .HasOne(re => re.Room)
                .WithMany(r => r.RoomEquipments)
                .HasForeignKey(re => re.RoomId);

            modelBuilder.Entity<RoomEquipment>()
                .HasOne(re => re.Equipment)
                .WithMany(e => e.RoomEquipments)
                .HasForeignKey(re => re.EquipmentId);


            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Room)
                .WithMany(r => r.Reservations)
                .HasForeignKey(r => r.RoomId)
                .OnDelete(DeleteBehavior.Cascade);


           



            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Room>()
                .HasIndex(r => r.Name)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(User => User.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Notification>()
                .Property(Notification => Notification.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Reservation>()
                .Property(Reservation => Reservation.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Room>()
                .Property(Room => Room.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<User>()
                .HasQueryFilter(user => user.DeletedAt == null);

            modelBuilder.Entity<Room>()
                .HasQueryFilter(room => room.DeletedAt == null);

            modelBuilder.Entity<Equipment>()
                .HasIndex(e => e.Name)
                .IsUnique();

           

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
