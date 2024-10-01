using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleParkingSystem.Models.Entities
{
    [PrimaryKey("Id")]
    public class ParkingSlot : BaseEntity
    {
        [ForeignKey("ParkArea")]
        public int ParkAreaId { get; set; }
        [ForeignKey("VehicleType")]
        public int VehicleTypeId { get; set; }
        public required string SlotNumber { get; set; }
        public bool IsOccupied { get; set; }
        public bool IsActive { get; set; }
        public ParkArea? ParkArea { get; set; }
        public VehicleType? VehicleType { get; set; }
        public ICollection<ParkingLog>? ParkingLogs { get; set; }
    }
}
