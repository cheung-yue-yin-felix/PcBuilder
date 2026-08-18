using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class MotherboardM2Configuration : IEntityTypeConfiguration<MotherboardM2>
{
    public void Configure(EntityTypeBuilder<MotherboardM2> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.Property(m2 => m2.SlotCount)
            .IsRequired();

        builder.HasOne<Motherboard>()
            .WithMany(m => m.M2Slots)
            .HasForeignKey(m2 => m2.MotherboardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(m2 => m2.FormFactors)
            .WithOne(ff => ff.MotherboardM2)
            .HasForeignKey(ff => ff.MotherboardM2Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(m2 => new { m2.MotherboardId, m2.Key, m2.PcieGeneration })
            .IsUnique();
    }
}