using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class ChassisFanMountOptionConfiguration : IEntityTypeConfiguration<ChassisFanMountOption>
{
    public void Configure(EntityTypeBuilder<ChassisFanMountOption> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.Property(o => o.SlotCount)
            .IsRequired();

        builder.HasOne<ChassisFanMount>()
            .WithMany(m => m.Options)
            .HasForeignKey(o => o.ChassisFanMountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}