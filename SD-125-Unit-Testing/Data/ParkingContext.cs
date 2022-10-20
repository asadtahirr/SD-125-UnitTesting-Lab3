using Microsoft.EntityFrameworkCore;
using SD_125_Unit_Testing.Models;

namespace SD_125_Unit_Testing.Data
{
    public class ParkingContext : DbContext
    {
        public virtual DbSet<Vehicle> Vehicles { get; set; }

        public virtual DbSet<Pass> Passes { get; set; }

        public virtual DbSet<ParkingSpot> ParkingSpots { get; set; }

        public virtual DbSet<Reservation> Reservations { get; set; }
    }
}
