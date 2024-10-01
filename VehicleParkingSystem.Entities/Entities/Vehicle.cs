using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleParkingSystem.Models.Entities
{
    [PrimaryKey("Id")]
    public class Vehicle : BaseEntity
    {
        public required string LicensePlate { get; set; }
        [ForeignKey("VehicleType")]
        public int VehicleTypeId { get; set; }
        public bool IsActive { get; set; }
        public required VehicleType VehicleType { get; set; }
        public ICollection<ParkingLog>? ParkingLogs { get; set; }        
    }
}
