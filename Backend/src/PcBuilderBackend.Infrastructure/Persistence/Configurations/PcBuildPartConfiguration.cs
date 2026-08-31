using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class PcBuildPartConfiguration : IEntityTypeConfiguration<PcBuildPart>
{
    public void Configure(EntityTypeBuilder<PcBuildPart> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.ToTable("PcBuildParts");

        builder.Property(part => part.Type).IsRequired();
        builder.Property(part => part.PartId).IsRequired();
        builder.Property(part => part.Quantity).IsRequired();

        builder.HasOne(part => part.PcBuild)
            .WithMany(build => build.Parts)
            .HasForeignKey(part => part.PcBuildId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(part => new { part.PcBuildId, part.Type, part.PartId })
            .IsUnique();
    }
}
