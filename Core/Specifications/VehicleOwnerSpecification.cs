using System.Linq;
using System;
using System.Linq.Expressions;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Specifications
{
    public class VehicleOwnerSpecification: BaseSpecification<Vehicle>
    {
        public VehicleOwnerSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.Include(e => e.VehicleOwners).ThenInclude(m => m.Owner));
        }
    }
}