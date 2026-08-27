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
    DbSet<ChassisFan> ChassisFans { get; set; }
    DbSet<ChassisPcieSlot> ChassisPcieSlots { get; set; }
    DbSet<ChassisMbFormFactor> ChassisMbFormFactors { get; set; }
    DbSet<ChassisRadiator> ChassisRadiators { get; set; }
    DbSet<ChassisPsuFormFactor> ChassisPsuFormFactors { get; set; }
    DbSet<CpuRamCompat> CpuRamCompats { get; set; }
    DbSet<CpuSupportChipset> CpuSupportChipsets { get; set; }
    DbSet<CpuCooler> CpuCoolers { get; set; }
    DbSet<CpuCoolerSocket> CpuCoolerSockets { get; set; }
    DbSet<Motherboard> Motherboards { get; set; }
    DbSet<MotherboardPcie> MotherboardPcieSlots { get; set; }
    DbSet<MotherboardM2> MotherboardM2Slots { get; set; }
    DbSet<MotherboardUsb> MotherboardUsbPorts { get; set; }
    DbSet<MotherboardM2FormFactor> MotherboardM2FormFactors { get; set; }
    DbSet<GraphicsCard> GraphicsCards { get; set; }
    DbSet<Psu> Psus { get; set; }
    DbSet<PsuCable> PsuCables { get; set; }
    DbSet<StorageDrive> StorageDrives { get; set; }
    DbSet<WiredNetworkAdapter> WiredNetworkAdapters { get; set; }
    DbSet<WirelessNetworkAdapter> WirelessNetworkAdapters { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
