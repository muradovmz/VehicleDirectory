using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.Property(v => v.Id).IsRequired();
            builder.Property(v => v.VinCode).IsRequired().HasMaxLength(50);
            builder.Property(v => v.StateNumberPlate).IsRequired();
            builder.Property(v => v.ManufactureDate).IsRequired();
            builder.HasOne(c => c.Color).WithMany().HasForeignKey(c => c.ColorId);
        }
    }
}