using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly DirectoryContext _context;
        public VehicleRepository(DirectoryContext context)
        {
            _context = context;
        }

        public async Task<Vehicle> GetVehicleByIdAsync(int id)
        {
            return await _context.Vehicles
            .Include(p=>p.Model)
            .Include(p=>p.Color)
            .FirstOrDefaultAsync(p=>p.Id==id);
        }

        public async Task<IReadOnlyList<Vehicle>> GetVehiclesAsync()
        {
            return await _context.Vehicles
            .Include(p=>p.Model)
            .Include(p=>p.Color)
            .ToListAsync();
        }
    }
}