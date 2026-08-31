using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Common.Interfaces;

public interface ICatalogRepository
{
    Task<Chassis?> GetChassisByIdAsync(Guid id);
    Task<ChassisFan?> GetChassisFanByIdAsync(Guid id);
    Task<Cpu?> GetCpuByIdAsync(Guid id);
    Task<CpuCooler?> GetCpuCoolerByIdAsync(Guid id);
    Task<GraphicsCard?> GetGraphicsCardByIdAsync(Guid id);
    Task<Motherboard?> GetMotherboardByIdAsync(Guid id);
    Task<Ram?> GetRamByIdAsync(Guid id);
    Task<StorageDrive?> GetStorageDeviceByIdAsync(Guid id);
    Task<Psu?> GetPowerSupplyByIdAsync(Guid id);
    Task<WiredNetworkAdapter?> GetWiredNetworkAdapterByIdAsync(Guid id);
    Task<WirelessNetworkAdapter?> GetWirelessNetworkAdapterByIdAsync(Guid id);
}

