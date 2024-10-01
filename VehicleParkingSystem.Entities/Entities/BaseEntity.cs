using System.ComponentModel.DataAnnotations.Schema;
using VehicleParkingSystem.Models.Interfaces;

namespace VehicleParkingSystem.Models.Entities
{
    public class BaseEntity : ITrackable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
