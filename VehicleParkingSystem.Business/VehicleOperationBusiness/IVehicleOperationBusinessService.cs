using VehicleParkingSystem.Models.Requests;
using VehicleParkingSystem.Models.Responses;

namespace VehicleParkingSystem.Business.VehicleOperationBusiness
{
    public interface IVehicleOperationBusinessService
    {
        Task<ReponseData<bool>> VehicleEntry(VehicleEntryRequest request);
        Task<ReponseData<string>> VehicleExit(int parkingLogId);
    }
}
