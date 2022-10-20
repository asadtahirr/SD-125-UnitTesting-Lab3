using Microsoft.EntityFrameworkCore;
using SD_125_Unit_Testing.Data;

namespace SD_125_Unit_Testing.Models
{
    public class ParkingHelper
    {
        private ParkingContext parkingContext;

        public ParkingHelper(ParkingContext context)
        {
            this.parkingContext = context;
        }

        public Pass CreatePass(string purchaser, bool premium, int capacity)
        {
            Pass newPass = new Pass();

            newPass.Purchaser = purchaser;

            newPass.Premium = premium;

            newPass.Capacity = capacity;

            parkingContext.Passes.Add(newPass);

            parkingContext.SaveChanges();

            return newPass;
        }

        public ParkingSpot CreateParkingSpot()
        {
            ParkingSpot newSpot = new ParkingSpot();

            newSpot.Occupied = false;

            parkingContext.ParkingSpots.Add(newSpot);

            return newSpot;
        }

        public async Task<bool> AddVehicleToPass(string passholderName, string vehicleLicence)
        {
            Pass pass = await parkingContext
                                .Passes
                                .Include(p => p.Vehicles)
                                .FirstOrDefaultAsync(p => p.Purchaser == passholderName);

            if (pass == null)
            {
                throw new Exception("Invalid passholder name");
            }

            Vehicle vehicle = await parkingContext.Vehicles.FirstOrDefaultAsync(v => v.Licence == vehicleLicence);

            if (vehicle == null)
            {
                throw new Exception("Invalid vehicle license");
            }

            if (pass.Vehicles.Count == pass.Capacity)
            {
                throw new Exception("Cannot add more vehicles to pass. Maximum capacity reached.");
            }

            pass.Vehicles.Add(vehicle);

            await parkingContext.SaveChangesAsync();

            return true;
        }
    }
}
