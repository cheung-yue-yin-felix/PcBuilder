using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class StorageDriveConfiguration : IEntityTypeConfiguration<StorageDrive>
{
    public void Configure(EntityTypeBuilder<StorageDrive> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.Property(sd => sd.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(sd => sd.CapacityGb)
            .IsRequired();

        builder.HasOne(sd => sd.Manufacturer)
            .WithMany(manufacturer => manufacturer.StorageDrives)
            .HasForeignKey(sd => sd.ManufacturerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}