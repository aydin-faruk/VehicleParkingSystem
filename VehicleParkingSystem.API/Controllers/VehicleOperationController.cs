using Microsoft.AspNetCore.Mvc;
using VehicleParkingSystem.Business.VehicleOperationBusiness;
using VehicleParkingSystem.Models.Requests;
using VehicleParkingSystem.Models.Responses;

namespace VehicleParkingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleOperationController : ControllerBase
    {
        private readonly IVehicleOperationBusinessService _vehicleOperationBusinessService;

        public VehicleOperationController(IVehicleOperationBusinessService vehicleOperationBusinessService)
        {
            _vehicleOperationBusinessService = vehicleOperationBusinessService;
        }

        [HttpPost("VehicleEntry")]
        public Task<ReponseData<bool>> VehicleEntry(VehicleEntryRequest request)
        {
            return _vehicleOperationBusinessService.VehicleEntry(request);
        }

        [HttpPost("VehicleExit")]
        public Task<ReponseData<string>> VehicleExit(int parkingLogId)
        {
            return _vehicleOperationBusinessService.VehicleExit(parkingLogId);
        }
    }
}
