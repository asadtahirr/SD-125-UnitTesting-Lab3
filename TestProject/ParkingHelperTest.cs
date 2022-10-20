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
        public ParkingHelperTest()
        {
            ParkingSpot parkingSpot1 = new ParkingSpot { ID = 1, Occupied = false };
            ParkingSpot parkingSpot2 = new ParkingSpot { ID = 1, Occupied = false };
            ParkingSpot parkingSpot3 = new ParkingSpot { ID = 1, Occupied = false };
            ParkingSpot parkingSpot4 = new ParkingSpot { ID = 1, Occupied = false };

            var parkingSpotData = new List<ParkingSpot>
            {
                parkingSpot1, parkingSpot2, parkingSpot3, parkingSpot4
            }.AsQueryable();

            var passData = new List<Pass>{}.AsQueryable();

            var vehicleData = new List<Vehicle>{}.AsQueryable();

            var reservationData = new List<Reservation>{}.AsQueryable();

            // mocking ParkingSpot DB set
            var mockParkingDbSet = new Mock<DbSet<ParkingSpot>>();

            mockParkingDbSet.As<IQueryable<ParkingSpot>>().Setup(m => m.Provider).Returns(parkingSpotData.Provider);
            mockParkingDbSet.As<IQueryable<ParkingSpot>>().Setup(m => m.Expression).Returns(parkingSpotData.Expression);
            mockParkingDbSet.As<IQueryable<ParkingSpot>>().Setup(m => m.ElementType).Returns(parkingSpotData.ElementType);
            mockParkingDbSet.As<IQueryable<ParkingSpot>>().Setup(m => m.GetEnumerator()).Returns(parkingSpotData.GetEnumerator);

            // mocking Reservation DB set
            var mockReservationDbSet = new Mock<DbSet<Reservation>>();

            mockReservationDbSet.As<IQueryable<Reservation>>().Setup(m => m.Provider).Returns(reservationData.Provider);
            mockReservationDbSet.As<IQueryable<Reservation>>().Setup(m => m.Expression).Returns(reservationData.Expression);
            mockReservationDbSet.As<IQueryable<Reservation>>().Setup(m => m.ElementType).Returns(reservationData.ElementType);
            mockReservationDbSet.As<IQueryable<Reservation>>().Setup(m => m.GetEnumerator()).Returns(reservationData.GetEnumerator);

            // mocking Vehicle DB set
            var mockVehicleDbSet = new Mock<DbSet<Vehicle>>();

            mockVehicleDbSet.As<IQueryable<Vehicle>>().Setup(m => m.Provider).Returns(vehicleData.Provider);
            mockVehicleDbSet.As<IQueryable<Vehicle>>().Setup(m => m.Expression).Returns(vehicleData.Expression);
            mockVehicleDbSet.As<IQueryable<Vehicle>>().Setup(m => m.ElementType).Returns(vehicleData.ElementType);
            mockVehicleDbSet.As<IQueryable<Vehicle>>().Setup(m => m.GetEnumerator()).Returns(vehicleData.GetEnumerator);

            // mocking Pass DB set
            var mockPassDbSet = new Mock<DbSet<Pass>>();

            mockPassDbSet.As<IQueryable<Pass>>().Setup(m => m.Provider).Returns(passData.Provider);
            mockPassDbSet.As<IQueryable<Pass>>().Setup(m => m.Expression).Returns(passData.Expression);
            mockPassDbSet.As<IQueryable<Pass>>().Setup(m => m.ElementType).Returns(passData.ElementType);
            mockPassDbSet.As<IQueryable<Pass>>().Setup(m => m.GetEnumerator()).Returns(passData.GetEnumerator);

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
    }
}