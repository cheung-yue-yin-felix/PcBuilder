using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Cpus.Queries;

public class FilterCpusHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<FilterCpusQuery, PagedResult<CpuListItemDto>>
{
    public async Task<PagedResult<CpuListItemDto>> Handle(
        FilterCpusQuery query, CancellationToken cancellationToken)
    {
        var filter = query.Request.Filter;
        var req = query.Request;

        var cpuQuery = context.Cpus
            .AsNoTracking()
            .WhereIf(!string.IsNullOrWhiteSpace(filter.Name),
                x => x.Name.Contains(filter.Name!))
            .WhereIf(filter.ManufacturerId.HasValue,
                x => x.ManufacturerId == filter.ManufacturerId)
            .WhereIf(filter.SocketId.HasValue,
                x => x.SocketId == filter.SocketId)
            .WhereIf(filter.SeriesId.HasValue,
                x => x.SeriesId == filter.SeriesId)
            .WhereIf(filter.PowerConsumptionWatts is not null,
                x => x.PowerConsumptionWatts >= filter.PowerConsumptionWatts!.Min &&
                     x.PowerConsumptionWatts <= filter.PowerConsumptionWatts!.Max)
            .WhereIf(filter.ThermalDesignPower is not null,
                x => x.ThermalDesignPower >= filter.ThermalDesignPower!.Min &&
                     x.ThermalDesignPower <= filter.ThermalDesignPower!.Max);

        if (filter.MotherboardId is not { } motherboardId)
        {
            return await cpuQuery
                .ApplySorting(req.SortFields, req.SortDirection)
                .ToPagedResultAsync<Cpu, CpuListItemDto>(
                    req.PageIndex,
                    req.PageSize,
                    mapper.ConfigurationProvider,
                    cancellationToken);
        }

        var motherboard = await context.Motherboards
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == motherboardId, cancellationToken);

        if (motherboard is null)
        {
            return new PagedResult<CpuListItemDto>
            {
                PageIndex = req.PageIndex,
                PageSize = req.PageSize,
                TotalCount = 0,
                Items = []
            };
        }

        // Domain CheckCpuCompatibility needs SupportedChipsets and is not EF-translatable.
        var cpus = await cpuQuery
            .Include(x => x.SupportedChipsets)
            .ToListAsync(cancellationToken);

        var compatible = cpus
            .Where(x => motherboard.CheckCpuCompatibility(x).Status != PartsCompatibility.Incompatible)
            .ToList();

        return new PagedResult<CpuListItemDto>
        {
            PageIndex = req.PageIndex,
            PageSize = req.PageSize,
            TotalCount = compatible.Count,
            Items = mapper.Map<List<CpuListItemDto>>(compatible
                .ApplySorting(req.SortFields, req.SortDirection)
                .Skip(req.PageIndex * req.PageSize)
                .Take(req.PageSize))
        };
    }
}
