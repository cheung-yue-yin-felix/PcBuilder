using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal static class BaseEntityConfiguration
{
    public static void ConfigureBaseEntity<TEntity, TKey>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : BaseEntity<TKey>
    {
        builder.HasKey(entity => entity.Id);
        builder.Property(entity => entity.CreatedAtUtc).IsRequired();
        builder.Property(entity => entity.ConcurrencyToken).IsConcurrencyToken();
    }

    public static void ConfigureGuidBaseEntity<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : BaseEntity<Guid>
    {
        builder.ConfigureBaseEntity<TEntity, Guid>();
        builder.Property(entity => entity.Id).ValueGeneratedNever();
    }
}