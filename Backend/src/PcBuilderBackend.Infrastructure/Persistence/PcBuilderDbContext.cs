using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence;

public class PcBuilderDbContext(DbContextOptions<PcBuilderDbContext> options) : DbContext(options)
{
    public DbSet<Cpu> Cpus => Set<Cpu>();

    public DbSet<Manufacturer> Manufacturers => Set<Manufacturer>();

    public DbSet<Socket> Sockets => Set<Socket>();

    public DbSet<Chipset> Chipsets => Set<Chipset>();

    public DbSet<Motherboard> Motherboards => Set<Motherboard>();

    public DbSet<MotherboardPcie> MotherboardPcieSlots => Set<MotherboardPcie>();

    public DbSet<MotherboardM2> MotherboardM2Slots => Set<MotherboardM2>();

    public DbSet<Gpu> Gpus => Set<Gpu>();

    public DbSet<GraphicsCard> GraphicsCards => Set<GraphicsCard>();

    public DbSet<GraphicsCardPowerConnector> GraphicsCardPowerConnectors => Set<GraphicsCardPowerConnector>();

    public DbSet<Ram> Rams => Set<Ram>();

    public DbSet<Psu> Psus => Set<Psu>();

    public DbSet<PsuCable> PsuCables => Set<PsuCable>();

    public DbSet<StorageDrive> StorageDrives => Set<StorageDrive>();

    public DbSet<WiredNetworkAdapter> WiredNetworkAdapters => Set<WiredNetworkAdapter>();

    public DbSet<WirelessNetworkAdapter> WirelessNetworkAdapters => Set<WirelessNetworkAdapter>();

    public DbSet<Chassis> Chassis => Set<Chassis>();

    public DbSet<ChassisFan> ChassisFans => Set<ChassisFan>();

    public DbSet<ChassisFanMount> ChassisFanMounts => Set<ChassisFanMount>();

    public DbSet<ChassisFanMountOption> ChassisFanMountOptions => Set<ChassisFanMountOption>();

    public DbSet<ChassisDriveBay> ChassisDriveBays => Set<ChassisDriveBay>();

    public DbSet<ChassisPcieSlot> ChassisPcieSlots => Set<ChassisPcieSlot>();

    public DbSet<ChassisRadiator> ChassisRadiators => Set<ChassisRadiator>();

    public DbSet<CpuCooler> CpuCoolers => Set<CpuCooler>();

    public DbSet<CpuCoolerSocket> CpuCoolerSockets => Set<CpuCoolerSocket>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigurePostgresEnums();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PcBuilderDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}