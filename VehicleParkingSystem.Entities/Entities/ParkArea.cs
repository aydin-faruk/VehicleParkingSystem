using Microsoft.EntityFrameworkCore;

namespace VehicleParkingSystem.Models.Entities
{
    [PrimaryKey("Id")]
    public class ParkArea : BaseEntity
    {        
        public required string Name { get; set; }
        public decimal RatePerHour { get; set; }
        public bool IsActive { get; set; }        
        public ICollection<ParkingSlot>? ParkingSlots { get; set; }
    }
}
