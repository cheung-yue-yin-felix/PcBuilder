using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Repositories;

public class CatalogRepository(PcBuilderDbContext context) : ICatalogRepository
{
    public async Task<Chassis?> GetChassisByIdAsync(Guid id) =>
        await context.Chassis
            .Include(c => c.DriveBays)
            .Include(c => c.FanMounts)
            .ThenInclude(fm => fm.Options)
            .Include(c => c.PcieSlots)
            .Include(c => c.PsuFormFactors)
            .Include(c => c.MbFormFactors)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<ChassisFan?> GetChassisFanByIdAsync(Guid id) =>
        await context.ChassisFans.FirstOrDefaultAsync(c => c.Id == id);

    public async Task<Cpu?> GetCpuByIdAsync(Guid id) =>
        await context.Cpus
            .Include(c => c.Socket)
            .Include(c => c.RamCompats)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<CpuCooler?> GetCpuCoolerByIdAsync(Guid id) =>
        await context.CpuCoolers
            .Include(c => c.CpuCoolerSockets)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<GraphicsCard?> GetGraphicsCardByIdAsync(Guid id) =>
        await context.GraphicsCards.FirstOrDefaultAsync(g => g.Id == id);

    public async Task<Motherboard?> GetMotherboardByIdAsync(Guid id) =>
        await context.Motherboards
            .Include(m => m.Socket)
            .Include(m => m.PcieSlots)
            .Include(m => m.M2Slots)
            .Include(m => m.UsbPorts)
            .FirstOrDefaultAsync(m => m.Id == id);

    public async Task<Ram?> GetRamByIdAsync(Guid id) =>
        await context.Rams.FirstOrDefaultAsync(r => r.Id == id);

    public async Task<StorageDrive?> GetStorageDeviceByIdAsync(Guid id) =>
        await context.StorageDrives.FirstOrDefaultAsync(s => s.Id == id);

    public async Task<Psu?> GetPowerSupplyByIdAsync(Guid id) =>
        await context.Psus.FirstOrDefaultAsync(p => p.Id == id);
    
    public async Task<WiredNetworkAdapter?> GetWiredNetworkAdapterByIdAsync(Guid id) =>
        await context.WiredNetworkAdapters.FirstOrDefaultAsync(w => w.Id == id);
    
    public async Task<WirelessNetworkAdapter?> GetWirelessNetworkAdapterByIdAsync(Guid id) =>
        await context.WirelessNetworkAdapters.FirstOrDefaultAsync(w => w.Id == id);
}