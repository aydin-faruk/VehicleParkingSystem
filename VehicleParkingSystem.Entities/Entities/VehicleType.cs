using Microsoft.EntityFrameworkCore;

namespace VehicleParkingSystem.Models.Entities
{
    [PrimaryKey("Id")]
    public class VehicleType : BaseEntity
    {
        public required string Name { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Vehicle>? Vehicles { get; set; }
        public ICollection<ParkingSlot>? ParkingSlots { get; set; }        
    }
}
