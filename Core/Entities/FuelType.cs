using System.Collections.Generic;

namespace Core.Entities
{
    public class FuelType:BaseEntity
    {
        public string FuelTypeName { get; set; }
        public ICollection<VehicleFuelType> VehicleFuelTypes { get; set; }
    }
}