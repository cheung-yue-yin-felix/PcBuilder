using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class ChipsetConfiguration : IEntityTypeConfiguration<Chipset>
{
    public void Configure(EntityTypeBuilder<Chipset> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.Property(chipset => chipset.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne(chipset => chipset.Manufacturer)
            .WithMany(manufacturer => manufacturer.Chipsets)
            .HasForeignKey(chipset => chipset.ManufacturerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(chipset => chipset.Socket)
            .WithMany()
            .HasForeignKey(chipset => chipset.SocketId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(chipset => chipset.ManufacturerId);
        builder.HasIndex(chipset => chipset.SocketId);
    }
}