namespace SD_125_Unit_Testing.Models
{
    public class Reservation
    {
        public int ID { get; set; }

        public int ParkingSpotID { get; set; }

        public virtual ParkingSpot ParkingSpot { get; set; }

        public int VehicleID { get; set; }

        public virtual Vehicle Vehicle { get; set; }

        public DateTime Expiry { get; set; }

        public bool IsCurrent { get; set; }
    }
}
