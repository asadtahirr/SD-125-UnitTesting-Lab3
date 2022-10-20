namespace SD_125_Unit_Testing.Models
{
    public class Vehicle
    {
        public int ID { get; set; }

        public int PassID { get; set; }

        public virtual Pass Pass { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }

        public bool Parked { get; set; }
    }
}
