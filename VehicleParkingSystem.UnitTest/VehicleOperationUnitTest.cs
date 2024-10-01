using Moq;
using VehicleParkingSystem.Business.ParkingLogBusiness;
using VehicleParkingSystem.Business.ParkingSlotBusiness;
using VehicleParkingSystem.Business.VehicleBusiness;
using VehicleParkingSystem.Business.VehicleOperationBusiness;
using VehicleParkingSystem.Models.Entities;
using VehicleParkingSystem.Models.Requests;

namespace VehicleParkingSystem.UnitTest
{
    public class VehicleOperationUnitTest
    {
        private readonly Mock<IVehicleBusinessService> _vehicleBusinessServiceMock;
        private readonly Mock<IParkingLogBusinessService> _parkingLogBusinessServiceMock;
        private readonly Mock<IParkingSlotBusinessService> _parkingSlotBusinessServiceMock;
        private readonly VehicleOperationBusinessService _vehicleOperationBusinessService;

        public VehicleOperationUnitTest()
        {
            _vehicleBusinessServiceMock = new Mock<IVehicleBusinessService>();
            _parkingLogBusinessServiceMock = new Mock<IParkingLogBusinessService>();
            _parkingSlotBusinessServiceMock = new Mock<IParkingSlotBusinessService>();

            _vehicleOperationBusinessService = new VehicleOperationBusinessService(
                _vehicleBusinessServiceMock.Object,
                _parkingLogBusinessServiceMock.Object,
                _parkingSlotBusinessServiceMock.Object
            );
        }

        [Test]
        public async Task VehicleEntry_SlotOccupied_ReturnsErrorMessage()
        {
            // Arrange
            var request = new VehicleEntryRequest { VehicleId = 1, ParkingSlotId = 1 };
            var parkingSlot = new ParkingSlot { Id = 1, SlotNumber = "A1", IsOccupied = true };

            _parkingSlotBusinessServiceMock.Setup(x => x.Get(request.ParkingSlotId))
                                        .ReturnsAsync(parkingSlot);

            // Action
            var result = await _vehicleOperationBusinessService.VehicleEntry(request);

            // Assert
            Assert.Multiple(() =>
            {                
                Assert.That(result.IsSuccessful, Is.False);
                Assert.That(result.Data, Is.False);
                Assert.That(result.Message, Is.EqualTo("A1 yeri doludur!"));
            });            
            _parkingLogBusinessServiceMock.Verify(x => x.Add(It.IsAny<ParkingLog>()), Times.Never);
            _parkingSlotBusinessServiceMock.Verify(x => x.Update(It.IsAny<ParkingSlot>()), Times.Never);
        }

        [Test]
        public async Task VehicleEntry_VehicleTypeMismatch_ReturnsErrorMessage()
        {
            // Arrange
            ParkingSlot parkingSlot = new()
            {
                Id = 1,
                SlotNumber = "A1",
                IsOccupied = false,
                VehicleType = new VehicleType { Id = 2, Name = "Motorsiklet" }
            };

            Vehicle vehicle = new()
            {
                Id = 1,
                LicensePlate = "34 FRK 34",
                VehicleType = new VehicleType { Id = 3, Name = "Taksi" }
            };

            VehicleEntryRequest request = new()
            {
                ParkingSlotId = 1,
                VehicleId = 1
            };

            _parkingSlotBusinessServiceMock.Setup(x => x.Get(request.ParkingSlotId)).ReturnsAsync(parkingSlot);
            _vehicleBusinessServiceMock.Setup(v => v.Get(request.VehicleId)).ReturnsAsync(vehicle);

            _parkingLogBusinessServiceMock.Setup(p => p.Add(It.IsAny<ParkingLog>())).Returns(Task.FromResult(false));
            _parkingSlotBusinessServiceMock.Setup(p => p.Update(It.IsAny<ParkingSlot>())).Returns(Task.FromResult(false));

            // Act
            var result = await _vehicleOperationBusinessService.VehicleEntry(request);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccessful, Is.False);
                Assert.That(result.Data, Is.False);
                Assert.That(result.Message, Is.EqualTo($"{vehicle.LicensePlate} plakalı aracınız {parkingSlot.SlotNumber} yeri için uygun değildir!"));
            });

            //_parkingSlotBusinessServiceMock.Verify(p => p.Update(It.IsAny<ParkingSlot>()), Times.Never);
            //_parkingLogBusinessServiceMock.Verify(p => p.Add(It.IsAny<ParkingLog>()), Times.Never);            
        }

        [Test]
        public async Task VehicleEntry_SuccessfulEntry_ReturnsTrue()
        {
            // Arrange
            var parkingSlot = new ParkingSlot
            {
                Id = 1,
                SlotNumber = "A1",
                IsOccupied = false,
                VehicleType = new VehicleType { Id = 3, Name = "Taksi" }  // Park yeri için uygun araç tipi
            };

            var vehicle = new Vehicle
            {
                Id = 1,
                LicensePlate = "34XYZ123",
                VehicleType = new VehicleType { Id = 3, Name = "Taksi" }  // Araç da aynı tip
            };

            var request = new VehicleEntryRequest
            {
                ParkingSlotId = 1,
                VehicleId = 1
            };

            _parkingSlotBusinessServiceMock.Setup(p => p.Get(It.IsAny<int>())).ReturnsAsync(parkingSlot);
            _vehicleBusinessServiceMock.Setup(v => v.Get(It.IsAny<int>())).ReturnsAsync(vehicle);

            _parkingLogBusinessServiceMock.Setup(p => p.Add(It.IsAny<ParkingLog>())).Returns(Task.FromResult(true));
            _parkingSlotBusinessServiceMock.Setup(p => p.Update(It.IsAny<ParkingSlot>())).Returns(Task.FromResult(true));

            // Act
            var result = await _vehicleOperationBusinessService.VehicleEntry(request);

            Assert.Multiple(() =>
            {                
                Assert.That(result.IsSuccessful, Is.True);
                Assert.That(result.Data, Is.True);
                Assert.That(result.Message, Is.Null);
            });
            _parkingLogBusinessServiceMock.Verify(p => p.Add(It.IsAny<ParkingLog>()), Times.Once);
            _parkingSlotBusinessServiceMock.Verify(p => p.Update(It.IsAny<ParkingSlot>()), Times.Once);
        }

        [Test]
        public async Task VehicleExit_SuccessfulExit_ReturnsParkingFee()
        {
            // Arrange
            int parkingLogId = 1;
            var parkingSlot = new ParkingSlot { Id = 1, SlotNumber = "A1", IsOccupied = true, ParkArea = new ParkArea { Name = "A", RatePerHour = 10 } };

            var parkingLog = new ParkingLog
            {
                VehicleId = 1,
                ParkingSlotId = 1,
                EntryTime = DateTime.Now.AddHours(-2),
                ParkingSlot = parkingSlot
            };

            _parkingLogBusinessServiceMock.Setup(x => x.Get(parkingLogId))
                                        .ReturnsAsync(parkingLog);

            // Act
            var result = await _vehicleOperationBusinessService.VehicleExit(parkingLogId);

            // Assert
            Assert.Multiple(() =>
            {                
                Assert.That(result.IsSuccessful, Is.True);
                Assert.That(result.Data, Does.StartWith("Park Ücretiniz:"));
            });
            _parkingSlotBusinessServiceMock.Verify(x => x.Update(parkingSlot), Times.Once);
        }
    }
}
