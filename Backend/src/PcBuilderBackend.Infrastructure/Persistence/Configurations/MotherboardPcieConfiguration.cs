using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class MotherboardPcieConfiguration : IEntityTypeConfiguration<MotherboardPcie>
{
    public void Configure(EntityTypeBuilder<MotherboardPcie> builder)
    {
        builder.ConfigureGuidBaseEntity();
    }
}