using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.UnitTests.Support;

public sealed class TestApplicationDbContext(DbContextOptions<TestApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<Manufacturer> Manufacturers { get; set; } = null!;
    public DbSet<Chipset> Chipsets { get; set; } = null!;
    public DbSet<Socket> Sockets { get; set; } = null!;
    public DbSet<CpuSeries> CpuSeries { get; set; } = null!;
    public DbSet<GpuSeries> GpuSeries { get; set; } = null!;
    public DbSet<Gpu> Gpus { get; set; } = null!;
    public DbSet<Cpu> Cpus { get; set; } = null!;
    public DbSet<Ram> Rams { get; set; } = null!;
    public DbSet<Chassis> Chassis { get; set; } = null!;
    public DbSet<ChassisDriveBay> ChassisDriveBays { get; set; } = null!;
    public DbSet<ChassisFanMount> ChassisFanMounts { get; set; } = null!;
    public DbSet<ChassisFanMountOption> ChassisFanMountOptions { get; set; } = null!;
    public DbSet<ChassisFan> ChassisFans { get; set; } = null!;
    public DbSet<ChassisPcieSlot> ChassisPcieSlots { get; set; } = null!;
    public DbSet<ChassisRadiator> ChassisRadiators { get; set; } = null!;
    public DbSet<CpuRamCompat> CpuRamCompats { get; set; } = null!;
    public DbSet<CpuSupportChipset> CpuSupportChipsets { get; set; } = null!;
    public DbSet<CpuCooler> CpuCoolers { get; set; } = null!;
    public DbSet<CpuCoolerSocket> CpuCoolerSockets { get; set; } = null!;
    public DbSet<Motherboard> Motherboards { get; set; } = null!;
    public DbSet<GraphicsCard> GraphicsCards { get; set; } = null!;
    public DbSet<Psu> Psus { get; set; } = null!;
    public DbSet<PsuCable> PsuCables { get; set; } = null!;
    public DbSet<StorageDrive> StorageDrives { get; set; } = null!;
    public DbSet<WiredNetworkAdapter> WiredNetworkAdapters { get; set; } = null!;
    public DbSet<WirelessNetworkAdapter> WirelessNetworkAdapters { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Psu>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.HasOne(x => x.Manufacturer)
                .WithMany(x => x.Psus)
                .HasForeignKey(x => x.ManufacturerId);
            builder.HasMany(x => x.Cables)
                .WithOne()
                .HasForeignKey(x => x.PsuId);
        });

        modelBuilder.Entity<PsuCable>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Chassis>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.HasMany(x => x.PsuFormFactors)
                .WithOne(x => x.Chassis)
                .HasForeignKey(x => x.ChassisId);
        });

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;
            if (!typeof(BaseEntity).IsAssignableFrom(clrType))
                continue;

            modelBuilder.Entity(clrType).Property(nameof(BaseEntity.Id)).ValueGeneratedNever();

            typeof(TestApplicationDbContext)
                .GetMethod(nameof(SetIsActiveQueryFilter), BindingFlags.NonPublic | BindingFlags.Static)!
                .MakeGenericMethod(clrType)
                .Invoke(null, [modelBuilder]);
        }
    }

    private static void SetIsActiveQueryFilter<TEntity>(ModelBuilder builder) where TEntity : BaseEntity
    {
        builder.Entity<TEntity>().HasQueryFilter(e => e.IsActive);
    }
}
