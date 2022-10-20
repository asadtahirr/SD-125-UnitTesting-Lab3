using Microsoft.EntityFrameworkCore;
using Moq;
using SD_125_Unit_Testing.Data;
using SD_125_Unit_Testing.Models;

namespace TestProject
{
    [TestClass]
    public class ParkingHelperTest
    {
        ParkingHelper ParkingHelper { get; set; }
        IQueryable<ParkingSpot> ParkingSpotData { get; set; }
        IQueryable<Pass> PassData { get; set; }
        IQueryable<Vehicle> VehicleData { get; set; }
        IQueryable<Reservation> ReservationData { get; set; }

        public ParkingHelperTest()
        {
            ParkingSpot parkingSpot1 = new ParkingSpot { ID = 1, Occupied = false };
            ParkingSpot parkingSpot2 = new ParkingSpot { ID = 2, Occupied = false };
            ParkingSpot parkingSpot3 = new ParkingSpot { ID = 3, Occupied = false };
            ParkingSpot parkingSpot4 = new ParkingSpot { ID = 4, Occupied = false };
            ParkingSpot parkingSpot5 = new ParkingSpot { ID = 5, Occupied = false };

            ParkingSpotData = new List<ParkingSpot>
            {
                parkingSpot1, parkingSpot2, parkingSpot3, parkingSpot4, parkingSpot5
            }.AsQueryable();

            Pass pass1 = new Pass { ID = 1, Purchaser = "A", Premium = false, Capacity = 1 };
            Pass pass2 = new Pass { ID = 2, Purchaser = "B", Premium = false, Capacity = 2 };
            Pass pass3 = new Pass { ID = 3, Purchaser = "C", Premium = false, Capacity = 3 };
            Pass pass4 = new Pass { ID = 4, Purchaser = "D", Premium = true, Capacity = 4 };
            Pass pass5 = new Pass { ID = 5, Purchaser = "E", Premium = true, Capacity = 5 };

            PassData = new List<Pass>
            {
                pass1, pass2, pass3, pass4, pass5
            }.AsQueryable();

            Vehicle vehicle1 = new Vehicle { ID = 1, Parked = false, Licence = "LA" };
            Vehicle vehicle2 = new Vehicle { ID = 2, Parked = false, Licence = "LB" };
            Vehicle vehicle3 = new Vehicle { ID = 3, Parked = false, Licence = "LC" };
            Vehicle vehicle4 = new Vehicle { ID = 4, Parked = false, Licence = "LD" };
            Vehicle vehicle5 = new Vehicle { ID = 5, Parked = false, Licence = "LE" };
            Vehicle vehicle6 = new Vehicle { ID = 6, Parked = false, Licence = "LF" };

            VehicleData = new List<Vehicle>
            {
                vehicle1, vehicle2, vehicle3, vehicle4, vehicle5
            }.AsQueryable();

            ReservationData = new List<Reservation>{}.AsQueryable();

            // mocking ParkingSpot DB set
            var mockParkingDbSet = new Mock<DbSet<ParkingSpot>>();

            mockParkingDbSet.As<IQueryable<ParkingSpot>>().Setup(m => m.Provider).Returns(ParkingSpotData.Provider);
            mockParkingDbSet.As<IQueryable<ParkingSpot>>().Setup(m => m.Expression).Returns(ParkingSpotData.Expression);
            mockParkingDbSet.As<IQueryable<ParkingSpot>>().Setup(m => m.ElementType).Returns(ParkingSpotData.ElementType);
            mockParkingDbSet.As<IQueryable<ParkingSpot>>().Setup(m => m.GetEnumerator()).Returns(ParkingSpotData.GetEnumerator);

            // mocking Reservation DB set
            var mockReservationDbSet = new Mock<DbSet<Reservation>>();

            mockReservationDbSet.As<IQueryable<Reservation>>().Setup(m => m.Provider).Returns(ReservationData.Provider);
            mockReservationDbSet.As<IQueryable<Reservation>>().Setup(m => m.Expression).Returns(ReservationData.Expression);
            mockReservationDbSet.As<IQueryable<Reservation>>().Setup(m => m.ElementType).Returns(ReservationData.ElementType);
            mockReservationDbSet.As<IQueryable<Reservation>>().Setup(m => m.GetEnumerator()).Returns(ReservationData.GetEnumerator);

            // mocking Vehicle DB set
            var mockVehicleDbSet = new Mock<DbSet<Vehicle>>();

            mockVehicleDbSet.As<IQueryable<Vehicle>>().Setup(m => m.Provider).Returns(VehicleData.Provider);
            mockVehicleDbSet.As<IQueryable<Vehicle>>().Setup(m => m.Expression).Returns(VehicleData.Expression);
            mockVehicleDbSet.As<IQueryable<Vehicle>>().Setup(m => m.ElementType).Returns(VehicleData.ElementType);
            mockVehicleDbSet.As<IQueryable<Vehicle>>().Setup(m => m.GetEnumerator()).Returns(VehicleData.GetEnumerator);

            // mocking Pass DB set
            var mockPassDbSet = new Mock<DbSet<Pass>>();

            mockPassDbSet.As<IQueryable<Pass>>().Setup(m => m.Provider).Returns(PassData.Provider);
            mockPassDbSet.As<IQueryable<Pass>>().Setup(m => m.Expression).Returns(PassData.Expression);
            mockPassDbSet.As<IQueryable<Pass>>().Setup(m => m.ElementType).Returns(PassData.ElementType);
            mockPassDbSet.As<IQueryable<Pass>>().Setup(m => m.GetEnumerator()).Returns(PassData.GetEnumerator);

            var mockParkingContext = new Mock<ParkingContext>();

            mockParkingContext.Setup(c => c.ParkingSpots).Returns(mockParkingDbSet.Object);
            mockParkingContext.Setup(c => c.Reservations).Returns(mockReservationDbSet.Object);
            mockParkingContext.Setup(c => c.Vehicles).Returns(mockVehicleDbSet.Object);
            mockParkingContext.Setup(c => c.Passes).Returns(mockPassDbSet.Object);

            ParkingHelper = new ParkingHelper(mockParkingContext.Object);
        }

        [TestMethod]
        public void CreateParkingSpotTest()
        {
            ParkingSpot parkingSpot = ParkingHelper.CreateParkingSpot();

            Assert.IsNotNull(parkingSpot);
        }

        [DataRow("Purchaser 1", false, 1)]
        [DataRow("Purchaser 2", true, 5)]
        [TestMethod]
        public void CreatePassTest(string purchaserInput, bool isPremiumInput, int capacityInput)
        {
            Pass pass = ParkingHelper.CreatePass(purchaserInput, isPremiumInput, capacityInput);

            Assert.IsNotNull(pass);

            Assert.AreEqual(pass.Purchaser, purchaserInput);

            Assert.AreEqual(pass.Premium, isPremiumInput);

            Assert.AreEqual(pass.Capacity, capacityInput);
        }

        [DataRow("A", "LA")]
        [DataRow("B", "LB")]
        [DataRow("X", "LF")]
        [DataRow("B", "LY")]
        [DataRow("A", "LC")]
        [DataRow("A", "LD")]
        [DataRow("A", "LE")]
        [DataRow("A", "LF")]
        [TestMethod]
        public async Task AddVehicleToPassTest(string passholderName, string vehicleLicence)
        {
            bool vehicleAddedToPass = await ParkingHelper.AddVehicleToPass(passholderName, vehicleLicence);

            Pass pass = PassData.FirstOrDefault(p => p.Purchaser == passholderName);
            Vehicle vehicle = VehicleData.FirstOrDefault(v => v.Licence == vehicleLicence);

            if (pass != null && vehicle != null)
            {
                Assert.IsTrue(vehicleAddedToPass);
            }
            else
            {
                Assert.IsFalse(vehicleAddedToPass);
            }
        }
    }
}