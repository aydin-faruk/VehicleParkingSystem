using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleParkingSystem.Models.Entities
{
    [PrimaryKey("Id")]
    public class ParkingLog : BaseEntity
    {
        [ForeignKey("Vehicle")]
        public int VehicleId { get; set; }
        [ForeignKey("ParkingSlot")]
        public int ParkingSlotId { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime ExitTime { get; set; }
        public Vehicle? Vehicle { get; set; }
        public ParkingSlot? ParkingSlot { get; set; }
    }
}
