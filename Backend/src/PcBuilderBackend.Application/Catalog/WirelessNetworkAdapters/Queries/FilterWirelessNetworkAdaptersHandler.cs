using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Queries;

public class FilterWirelessNetworkAdaptersHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<FilterWirelessNetworkAdaptersQuery, PagedResult<WirelessNetworkAdapterDto>>
{
    public async Task<PagedResult<WirelessNetworkAdapterDto>> Handle(
        FilterWirelessNetworkAdaptersQuery query,
        CancellationToken cancellationToken)
    {
        var filter = query.Request.Filter;
        var req = query.Request;

        var queryable = context.WirelessNetworkAdapters.AsNoTracking()
            .Include(x => x.Manufacturer)
            .WhereIf(!string.IsNullOrWhiteSpace(filter.Name), x => x.Name.Contains(filter.Name!))
            .WhereIf(filter.ManufacturerId.HasValue, x => x.ManufacturerId == filter.ManufacturerId)
            .WhereIf(filter.WifiStandard.HasValue, x => x.WifiStandard == filter.WifiStandard)
            .WhereIf(filter.BluetoothVersion.HasValue, x => x.BluetoothVersion == filter.BluetoothVersion)
            .WhereIf(filter.HostInterface.HasValue, x => x.HostInterface == filter.HostInterface)
            .WhereIf(filter.MaxSpeedMbps is not null,
                x => x.MaxSpeedMbps >= filter.MaxSpeedMbps!.Min && x.MaxSpeedMbps <= filter.MaxSpeedMbps!.Max)
            .WhereIf(filter.MaxSpeedMbps5G is not null,
                x => x.MaxSpeedMbps5G >= filter.MaxSpeedMbps5G!.Min && x.MaxSpeedMbps5G <= filter.MaxSpeedMbps5G!.Max)
            .WhereIf(filter.MaxSpeedMbps6G is not null,
                x => x.MaxSpeedMbps6G >= filter.MaxSpeedMbps6G!.Min && x.MaxSpeedMbps6G <= filter.MaxSpeedMbps6G!.Max)
            .WhereIf(filter.PcieSlotType.HasValue, x => x.PcieSlotType == filter.PcieSlotType)
            .WhereIf(filter.Key.HasValue, x => x.Key == filter.Key)
            .WhereIf(filter.M2FormFactor.HasValue, x => x.M2FormFactor == filter.M2FormFactor)
            .WhereIf(filter.UsbVersion.HasValue, x => x.UsbVersion == filter.UsbVersion)
            .WhereIf(filter.UsbType.HasValue, x => x.UsbType == filter.UsbType);

        if (!filter.MotherboardId.HasValue)
        {
            return await queryable
                .ApplySorting(req.SortFields, req.SortDirection)
                .ToPagedResultAsync<WirelessNetworkAdapter, WirelessNetworkAdapterDto>(
                    req.PageIndex,
                    req.PageSize,
                    mapper.ConfigurationProvider,
                    cancellationToken);
        }

        var motherboard = await context.Motherboards
            .AsNoTracking()
            .Include(m => m.PcieSlots)
            .Include(m => m.UsbPorts)
            .Include(m => m.M2Slots)
            .ThenInclude(s => s.FormFactors)
            .FirstOrDefaultAsync(m => m.Id == filter.MotherboardId.Value, cancellationToken);

        if (motherboard is null)
            return PagedResult<WirelessNetworkAdapterDto>.Empty(req);

        var adapters = (await queryable.ToListAsync(cancellationToken))
            .Where(x =>
                motherboard.CheckWirelessNetworkAdapterCompatibility(x).Status != PartsCompatibility.Incompatible)
            .ApplySorting(req.SortFields, req.SortDirection)
            .ToList();

        return new PagedResult<WirelessNetworkAdapterDto>
        {
            PageIndex = req.PageIndex,
            PageSize = req.PageSize,
            TotalCount = adapters.Count,
            Items = mapper.Map<List<WirelessNetworkAdapterDto>>(adapters
                .Skip(req.PageIndex * req.PageSize)
                .Take(req.PageSize))
        };
    }
}
