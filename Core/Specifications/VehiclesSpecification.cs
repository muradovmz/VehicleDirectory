using System.Linq;
using System;
using System.Linq.Expressions;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Specifications
{
    public class VehiclesSpecification : BaseSpecification<Vehicle>
    {
        public VehiclesSpecification(VehicleSpecParams vehicleParams)
            : base(x =>
                (string.IsNullOrEmpty(vehicleParams.Search) || x.StateNumberPlate.ToLower().Contains(vehicleParams.Search)) &&
                 (!vehicleParams.ManufacturerId.HasValue || x.Model.ManufacturerId == vehicleParams.ManufacturerId) &&
                 (!vehicleParams.ModelId.HasValue || x.ModelId == vehicleParams.ModelId) &&
                 (!vehicleParams.ColorId.HasValue || x.ColorId == vehicleParams.ColorId) &&
                 (!vehicleParams.FuelTypeId.HasValue || x.VehicleFuelTypes.Any(c => c.FuelType.Id == vehicleParams.FuelTypeId))
            )
        {
            AddInclude(x => x.Include(e => e.Color));
            AddInclude(x => x.Include(e => e.Model).ThenInclude(m => m.Manufacturer));
            AddInclude(x => x.Include(e => e.VehicleOwners).ThenInclude(m => m.Owner));
            AddInclude(x => x.Include(e => e.VehicleFuelTypes).ThenInclude(m => m.FuelType));
            AddInclude(x=>x.Include(e=>e.Photos));
            AddOrderBy(x => x.Model.ModelNameEN);
            ApplyPaging(vehicleParams.PageSize * (vehicleParams.PageIndex - 1), vehicleParams.PageSize);

            if (!string.IsNullOrEmpty(vehicleParams.Sort))
            {
                switch (vehicleParams.Sort)
                {
                    case "manufactureDateAsc":
                        AddOrderBy(d => d.ManufactureDate);
                        break;
                    case "manufactureDateDesc":
                        AddOrderByDescending(d => d.ManufactureDate);
                        break;
                    default:
                        AddOrderBy(x => x.Model.ModelNameEN);
                        break;
                }
            }

        }

        public VehiclesSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.Include(e => e.Color));
            AddInclude(x => x.Include(e => e.Model).ThenInclude(m => m.Manufacturer));
            AddInclude(x => x.Include(e => e.VehicleOwners).ThenInclude(m => m.Owner));
            AddInclude(x => x.Include(e => e.VehicleFuelTypes).ThenInclude(m => m.FuelType));
            AddInclude(x=>x.Include(e=>e.Photos));
        }
    }
}