using System.Linq;
using Core.Entities;

namespace Core.Specifications
{
    public class VehicleWithFiltersForCountSpecification : BaseSpecification<Vehicle>
    {
        public VehicleWithFiltersForCountSpecification(VehicleSpecParams vehicleParams)
            : base(x =>
                (string.IsNullOrEmpty(vehicleParams.Search) || x.StateNumberPlate.ToLower().Contains(vehicleParams.Search)) &&
                 (!vehicleParams.ManufacturerId.HasValue || x.Model.ManufacturerId == vehicleParams.ManufacturerId) &&
                 (!vehicleParams.ModelId.HasValue || x.ModelId == vehicleParams.ModelId) &&
                 (!vehicleParams.ColorId.HasValue || x.ColorId == vehicleParams.ColorId) &&
                 (!vehicleParams.FuelTypeId.HasValue || x.VehicleFuelTypes.Any(c => c.FuelType.Id == vehicleParams.FuelTypeId))
            )
        {
        }
    }
}