using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class ManufacturerConfiguration : IEntityTypeConfiguration<Manufacturer>
{
    public void Configure(EntityTypeBuilder<Manufacturer> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.Property(manufacturer => manufacturer.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(manufacturer => manufacturer.Name)
            .IsUnique();
    }
}