using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Queries;

public class FilterWiredNetworkAdaptersHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<FilterWiredNetworkAdaptersQuery, PagedResult<WiredNetworkAdapterDto>>
{
    public async Task<PagedResult<WiredNetworkAdapterDto>> Handle(
        FilterWiredNetworkAdaptersQuery query,
        CancellationToken cancellationToken)
    {
        var filter = query.Request.Filter;
        var req = query.Request;

        var queryable = context.WiredNetworkAdapters.AsNoTracking()
            .Include(x => x.Manufacturer)
            .WhereIf(!string.IsNullOrWhiteSpace(filter.Name), x => x.Name.Contains(filter.Name!))
            .WhereIf(filter.ManufacturerId.HasValue, x => x.ManufacturerId == filter.ManufacturerId)
            .WhereIf(filter.HostInterface.HasValue, x => x.HostInterface == filter.HostInterface)
            .WhereIf(filter.MaxSpeedMbps is not null,
                x => x.MaxSpeedMbps >= filter.MaxSpeedMbps!.Min && x.MaxSpeedMbps <= filter.MaxSpeedMbps!.Max)
            .WhereIf(filter.UsbVersion.HasValue, x => x.UsbVersion == filter.UsbVersion)
            .WhereIf(filter.UsbType.HasValue, x => x.UsbType == filter.UsbType)
            .WhereIf(filter.PcieSlotType.HasValue, x => x.PcieSlotType == filter.PcieSlotType);

        if (!filter.MotherboardId.HasValue)
        {
            return await queryable
                .ApplySorting(req.SortFields, req.SortDirection)
                .ToPagedResultAsync<WiredNetworkAdapter, WiredNetworkAdapterDto>(
                    req.PageIndex,
                    req.PageSize,
                    mapper.ConfigurationProvider,
                    cancellationToken);
        }

        var motherboard = await context.Motherboards
            .AsNoTracking()
            .Include(m => m.PcieSlots)
            .Include(m => m.UsbPorts)
            .FirstOrDefaultAsync(m => m.Id == filter.MotherboardId.Value, cancellationToken);

        if (motherboard is null)
            return PagedResult<WiredNetworkAdapterDto>.Empty(req);

        var adapters = (await queryable.ToListAsync(cancellationToken))
            .Where(x => motherboard.CheckWiredNetworkAdapterCompatibility(x).Status != PartsCompatibility.Incompatible)
            .ApplySorting(req.SortFields, req.SortDirection)
            .ToList();

        return new PagedResult<WiredNetworkAdapterDto>
        {
            PageIndex = req.PageIndex,
            PageSize = req.PageSize,
            TotalCount = adapters.Count,
            Items = mapper.Map<List<WiredNetworkAdapterDto>>(adapters
                .Skip(req.PageIndex * req.PageSize)
                .Take(req.PageSize))
        };
    }
}
