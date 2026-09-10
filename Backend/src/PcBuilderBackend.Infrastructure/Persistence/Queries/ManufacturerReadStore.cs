using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.MasterData.Manufacturers;
using PcBuilderBackend.Application.MasterData.Manufacturers.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Infrastructure.Persistence.Queries;

public sealed class ManufacturerReadStore(PcBuilderDbContext db, IMapper mapper) : IManufacturerReadStore
{
    public Task<List<ManufacturerDto>> ListByProductTypeAsync(
        ProductType productType,
        CancellationToken cancellationToken)
    {
        var manufacturers = db.Manufacturers.AsNoTracking();

        var filtered = productType switch
        {
            ProductType.Chassis => manufacturers.Where(m => m.Chassis.Any()),
            ProductType.ChassisFan => manufacturers.Where(m => m.ChassisFans.Any()),
            ProductType.Chipset => manufacturers.Where(m => m.Chipsets.Any()),
            ProductType.Cpu => manufacturers.Where(m => m.Cpus.Any()),
            ProductType.CpuCooler => manufacturers.Where(m => m.CpuCoolers.Any()),
            ProductType.CpuSeries => manufacturers.Where(m => m.CpuSeries.Any()),
            ProductType.Gpu => manufacturers.Where(m => m.Gpus.Any()),
            ProductType.GpuSeries => manufacturers.Where(m => m.GpuSeries.Any()),
            ProductType.GraphicsCard => manufacturers.Where(m => m.GraphicsCards.Any()),
            ProductType.Motherboard => manufacturers.Where(m => m.Motherboards.Any()),
            ProductType.Psu => manufacturers.Where(m => m.Psus.Any()),
            ProductType.Ram => manufacturers.Where(m => m.Rams.Any()),
            ProductType.Socket => manufacturers.Where(m => m.Sockets.Any()),
            ProductType.StorageDrive => manufacturers.Where(m => m.StorageDrives.Any()),
            ProductType.WiredNetworkAdapter => manufacturers.Where(m => m.WiredNetworkAdapters.Any()),
            ProductType.WirelessNetworkAdapter => manufacturers.Where(m => m.WirelessNetworkAdapters.Any()),
            _ => throw new ArgumentOutOfRangeException(nameof(productType), productType, null)
        };

        return filtered
            .OrderBy(m => m.Name)
            .ProjectTo<ManufacturerDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
