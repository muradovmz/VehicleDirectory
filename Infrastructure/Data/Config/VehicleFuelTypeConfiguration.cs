using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class VehicleFuelTypeConfiguration: IEntityTypeConfiguration<VehicleFuelType>
    {
        public void Configure(EntityTypeBuilder<VehicleFuelType> builder)
        {
            builder.HasKey(vo => new {vo.VehicleId, vo.FuelTypeId});
            builder.HasOne(vo=>vo.Vehicle).WithMany(v => v.VehicleFuelTypes).HasForeignKey(vo => vo.VehicleId);
            builder.HasOne(vo=>vo.FuelType).WithMany(v => v.VehicleFuelTypes).HasForeignKey(vo => vo.FuelTypeId);
        }
    }
}