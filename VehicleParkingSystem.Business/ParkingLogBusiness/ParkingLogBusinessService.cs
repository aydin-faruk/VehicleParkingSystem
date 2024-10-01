using Microsoft.EntityFrameworkCore;
using VehicleParkingSystem.Data.Context;
using VehicleParkingSystem.Models.Entities;

namespace VehicleParkingSystem.Business.ParkingLogBusiness
{
    public class ParkingLogBusinessService : IParkingLogBusinessService
    {
        private readonly VehicleParkingSystemDBContext _dbContext;

        public ParkingLogBusinessService(VehicleParkingSystemDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ParkingLog> Get(int id)
        {
            ParkingLog? parkingLog = await _dbContext.ParkingLogs.Include(x => x.ParkingSlot)
                                                        .ThenInclude(x => x.ParkArea)
                                                        .Where(x => x.Id == id)
                                                        .FirstOrDefaultAsync();

            return parkingLog;
        }

        public Task<bool> Add(ParkingLog parkingLog)
        {
            _dbContext.ParkingLogs.Add(parkingLog);
            _dbContext.SaveChanges();

            return Task.FromResult(true);
        }

        public Task<bool> Update(ParkingLog parkingLog)
        {
            _dbContext.ParkingLogs.Update(parkingLog);
            _dbContext.SaveChanges();

            return Task.FromResult(true);
        }
    }
}
