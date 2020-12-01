using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class VehicleOwnerConfiguration : IEntityTypeConfiguration<VehicleOwner>
    {
        public void Configure(EntityTypeBuilder<VehicleOwner> builder)
        {
            builder.HasKey(vo => new {vo.VehicleId, vo.OwnerId});
            builder.HasOne(vo=>vo.Vehicle).WithMany(v => v.VehicleOwners).HasForeignKey(vo => vo.VehicleId);
            builder.HasOne(vo=>vo.Owner).WithMany(v => v.VehicleOwners).HasForeignKey(vo => vo.OwnerId);
        }
    }
}