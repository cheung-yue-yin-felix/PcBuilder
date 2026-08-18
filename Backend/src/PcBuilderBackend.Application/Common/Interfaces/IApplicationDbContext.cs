using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Manufacturer> Manufacturers { get; set; }
    DbSet<Chipset> Chipsets { get; set; }
    DbSet<Socket> Sockets { get; set; }
    DbSet<CpuSeries> CpuSeries { get; set; }
    DbSet<GpuSeries> GpuSeries { get; set; }
    DbSet<Gpu> Gpus { get; set; }
    DbSet<Cpu> Cpus { get; set; }
    DbSet<Ram> Rams { get; set; }
    DbSet<Chassis> Chassis { get; set; }
    DbSet<ChassisDriveBay> ChassisDriveBays { get; set; }
    DbSet<ChassisFanMount> ChassisFanMounts { get; set; }
    DbSet<ChassisFanMountOption> ChassisFanMountOptions { get; set; }
    DbSet<ChassisPcieSlot> ChassisPcieSlots { get; set; }
    DbSet<ChassisRadiator> ChassisRadiators { get; set; }
    DbSet<CpuRamCompat> CpuRamCompats { get; set; }
    DbSet<CpuSupportChipset> CpuSupportChipsets { get; set; }
    DbSet<Motherboard> Motherboards { get; set; }
    DbSet<GraphicsCard> GraphicsCards { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
