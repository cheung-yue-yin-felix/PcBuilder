using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Common.Interfaces;

public interface ICatalogRepository
{
    Task<Chassis?> GetChassisByIdAsync(Guid id);
    Task<ChassisFan?> GetChassisFanByIdAsync(Guid id);
    Task<List<ChassisFan>> GetChassisFansByIdsAsync(IReadOnlyCollection<Guid> ids);
    Task<Cpu?> GetCpuByIdAsync(Guid id);
    Task<CpuCooler?> GetCpuCoolerByIdAsync(Guid id);
    Task<GraphicsCard?> GetGraphicsCardByIdAsync(Guid id);
    Task<Motherboard?> GetMotherboardByIdAsync(Guid id);
    Task<Ram?> GetRamByIdAsync(Guid id);
    Task<StorageDrive?> GetStorageDeviceByIdAsync(Guid id);
    Task<List<StorageDrive>> GetStorageDevicesByIdsAsync(IReadOnlyCollection<Guid> ids);
    Task<Psu?> GetPowerSupplyByIdAsync(Guid id);
    Task<WiredNetworkAdapter?> GetWiredNetworkAdapterByIdAsync(Guid id);
    Task<List<WiredNetworkAdapter>> GetWiredNetworkAdaptersByIdsAsync(IReadOnlyCollection<Guid> ids);
    Task<WirelessNetworkAdapter?> GetWirelessNetworkAdapterByIdAsync(Guid id);
    Task<List<WirelessNetworkAdapter>> GetWirelessNetworkAdaptersByIdsAsync(IReadOnlyCollection<Guid> ids);
}
