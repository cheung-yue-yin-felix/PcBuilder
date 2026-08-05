using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class MotherboardM2FormFactorConfiguration : IEntityTypeConfiguration<MotherboardM2FormFactor>
{
    public void Configure(EntityTypeBuilder<MotherboardM2FormFactor> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.HasOne(ff => ff.MotherboardM2)
            .WithMany(m2 => m2.FormFactors)
            .HasForeignKey(ff => ff.MotherboardM2Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(ff => new { ff.MotherboardM2Id, ff.FormFactor })
            .IsUnique();
    }
}
