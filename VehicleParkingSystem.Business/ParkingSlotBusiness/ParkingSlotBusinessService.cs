using Microsoft.EntityFrameworkCore;
using VehicleParkingSystem.Data.Context;
using VehicleParkingSystem.Models.Entities;

namespace VehicleParkingSystem.Business.ParkingSlotBusiness
{
    public class ParkingSlotBusinessService : IParkingSlotBusinessService
    {
        private readonly VehicleParkingSystemDBContext _dbContext;

        public ParkingSlotBusinessService(VehicleParkingSystemDBContext dbContext)
        {
            _dbContext = dbContext;
        }        

        public async Task<ParkingSlot> Get(int id)
        {
            ParkingSlot? parkingSlot = await _dbContext.ParkingSlots.Include(x => x.ParkArea)
                                                                    .Where(x => x.Id == id && x.IsActive)
                                                                    .FirstOrDefaultAsync();

            return parkingSlot;
        }

        public Task<bool> Add(ParkingSlot parkingSlot)
        {
            _dbContext.ParkingSlots.Add(parkingSlot);
            _dbContext.SaveChanges();

            return Task.FromResult(true);
        }

        public Task<bool> Update(ParkingSlot parkingSlot)
        {
            _dbContext.ParkingSlots.Update(parkingSlot);
            _dbContext.SaveChanges();

            return Task.FromResult(true);
        }
    }
}
