using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class ModelConfiguration : IEntityTypeConfiguration<Model>
    {
        public void Configure(EntityTypeBuilder<Model> builder)
        {
            builder.Property(m => m.ModelNameEN).IsRequired();
            builder.Property(m => m.ModelNameGE).IsRequired();
            builder.HasOne(x => x.Manufacturer).WithMany(x =>x.Models).HasForeignKey(p => p.ManufacturerId);
        }
    }
}