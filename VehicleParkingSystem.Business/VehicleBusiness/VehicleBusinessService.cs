using Microsoft.EntityFrameworkCore;
using VehicleParkingSystem.Data.Context;
using VehicleParkingSystem.Models.Entities;

namespace VehicleParkingSystem.Business.VehicleBusiness
{
    public class VehicleBusinessService : IVehicleBusinessService
    {
        private readonly VehicleParkingSystemDBContext _dbContext;

        public VehicleBusinessService(VehicleParkingSystemDBContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<Vehicle> Get(int id)
        {
            Vehicle? vehicle = await _dbContext.Vehicles.Include(x => x.VehicleType)
                                                        .Where(x => x.Id == id && x.IsActive)
                                                        .FirstOrDefaultAsync();

            return vehicle;
        }

        public Task<bool> Add(Vehicle vehicle)
        {
            _dbContext.Vehicles.Add(vehicle);
            _dbContext.SaveChanges();

            return Task.FromResult(true);
        }

        public Task<bool> Update(Vehicle vehicle)
        {
            _dbContext.Vehicles.Update(vehicle);
            _dbContext.SaveChanges();

            return Task.FromResult(true);
        }
    }
}
