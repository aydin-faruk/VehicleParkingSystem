using VehicleParkingSystem.Business.Helpers;
using VehicleParkingSystem.Business.ParkingLogBusiness;
using VehicleParkingSystem.Business.ParkingSlotBusiness;
using VehicleParkingSystem.Business.VehicleBusiness;
using VehicleParkingSystem.Models.Entities;
using VehicleParkingSystem.Models.Requests;
using VehicleParkingSystem.Models.Responses;

namespace VehicleParkingSystem.Business.VehicleOperationBusiness
{
    public class VehicleOperationBusinessService : IVehicleOperationBusinessService
    {
        private readonly IVehicleBusinessService _vehicleBusinessService;
        private readonly IParkingLogBusinessService _parkingLogBusinessService;
        private readonly IParkingSlotBusinessService _parkingSlotBusinessService;
        
        public VehicleOperationBusinessService(IVehicleBusinessService vehicleBusinessService,
            IParkingLogBusinessService parkingLogBusinessService, IParkingSlotBusinessService parkingSlotBusinessService)
        {
            _vehicleBusinessService = vehicleBusinessService;
            _parkingLogBusinessService = parkingLogBusinessService;
            _parkingSlotBusinessService = parkingSlotBusinessService;
        }

        public async Task<ReponseData<bool>> VehicleEntry(VehicleEntryRequest request)
        {
            ReponseData<bool> response = new();

            ParkingSlot parkingSlot = await _parkingSlotBusinessService.Get(request.ParkingSlotId);

            if (parkingSlot.IsOccupied)
            {
                response = new() { Data = false, IsSuccessful = false, Message = $"{parkingSlot.SlotNumber} yeri doludur!" };
                return await Task.FromResult(response);
            }

            var vehicle = await _vehicleBusinessService.Get(request.VehicleId);
            if (vehicle.VehicleType.Id != parkingSlot.VehicleType.Id)
            {
                response = new() { Data = false, IsSuccessful = false, Message = $"{vehicle.LicensePlate} plakalı aracınız {parkingSlot?.SlotNumber} yeri için uygun değildir!" };
                return await Task.FromResult(response);
            }

            ParkingLog parkingLog = new()
            {
                VehicleId = request.VehicleId,
                ParkingSlotId = request.ParkingSlotId,
                EntryTime = DateTime.Now
            };

            await _parkingLogBusinessService.Add(parkingLog);

            parkingSlot.IsOccupied = true;
            await _parkingSlotBusinessService.Update(parkingSlot);

            response = new() { Data = true, IsSuccessful = true };
            return await Task.FromResult(response);
        }

        public async Task<ReponseData<string>> VehicleExit(int parkingLogId)
        {
            ReponseData<string> response = new();

            ParkingLog parkingLog = await _parkingLogBusinessService.Get(parkingLogId);

            parkingLog.ParkingSlot.IsOccupied = false;
            parkingLog.ExitTime = DateTime.Now;
            await _parkingSlotBusinessService.Update(parkingLog.ParkingSlot);


            decimal ratePerHour = parkingLog.ParkingSlot.ParkArea.RatePerHour;
            decimal parkingFee = CalculateParkingFeeHelper.CalculateParkingFee(parkingLog.EntryTime, parkingLog.ExitTime, ratePerHour);

            response = new() { Data = $"Park Ücretiniz: {parkingFee}", IsSuccessful = true };
            return await Task.FromResult(response);
        }
    }
}
