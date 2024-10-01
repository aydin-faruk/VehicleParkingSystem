using Microsoft.EntityFrameworkCore;
using VehicleParkingSystem.Models.Entities;
using VehicleParkingSystem.Models.Interfaces;

namespace VehicleParkingSystem.Data.Context
{
    public class VehicleParkingSystemDBContext : DbContext
    {
        public required DbSet<Vehicle> Vehicles { get; set; }
        public required DbSet<VehicleType> VehicleTypes { get; set; }
        public required DbSet<ParkArea> ParkAreas { get; set; }
        public required DbSet<ParkingSlot> ParkingSlots { get; set; }
        public required DbSet<ParkingLog> ParkingLogs { get; set; }

        public VehicleParkingSystemDBContext(DbContextOptions<VehicleParkingSystemDBContext> options)
            : base(options)
        {
        }

        public override int SaveChanges()
        {
            OnBeforeSave();

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            OnBeforeSave();

            return await base.SaveChangesAsync(cancellationToken);
        }

        private void OnBeforeSave()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.Entity is ITrackable trackable)
                {
                    var now = DateTime.UtcNow;

                    if (entry.State == EntityState.Added)
                    {
                        trackable.CreatedAt = now;
                    }

                    trackable.UpdatedAt = now;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParkingLog>()
                        .HasOne(pl => pl.Vehicle)
                        .WithMany(v => v.ParkingLogs)
                        .HasForeignKey(pl => pl.VehicleId)
                        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
