using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Memories.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Memories.Queries;

public class FilterMemoriesHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<FilterMemoriesQuery, PagedResult<RamDto>>
{
    public async Task<PagedResult<RamDto>> Handle(
        FilterMemoriesQuery query,
        CancellationToken cancellationToken)
    {
        var filter = query.Request.Filter;
        var req = query.Request;

        var queryable = context.Rams.AsNoTracking()
            .WhereIf(!string.IsNullOrWhiteSpace(filter.Name), x => x.Name.Contains(filter.Name!))
            .WhereIf(filter.ManufacturerId.HasValue, x => x.ManufacturerId == filter.ManufacturerId)
            .WhereIf(!string.IsNullOrWhiteSpace(filter.Color), x => x.Color.Contains(filter.Color!))
            .WhereIf(filter.DdrGeneration.HasValue, x => x.DdrGeneration == filter.DdrGeneration)
            .WhereIf(filter.RamFormFactor.HasValue, x => x.RamFormFactor == filter.RamFormFactor)
            .WhereIf(filter.RamRank.HasValue, x => x.RamRank == filter.RamRank)
            .WhereIf(filter.MemorySizePerStickGb.HasValue,
                x => x.MemorySizePerStickGb == filter.MemorySizePerStickGb)
            .WhereIf(filter.TotalMemorySizeGb.HasValue,
                x => x.TotalMemorySizeGb == filter.TotalMemorySizeGb)
            .WhereIf(filter.ModulesCount.HasValue, x => x.ModulesCount == filter.ModulesCount)
            .WhereIf(filter.MaxMemorySpeedMts.HasValue,
                x => x.MaxMemorySpeedMts == filter.MaxMemorySpeedMts)
            .WhereIf(filter.HeightMm is not null,
                x => x.HeightMm >= filter.HeightMm!.Min && x.HeightMm <= filter.HeightMm!.Max);

        if (!filter.CpuId.HasValue && !filter.MotherboardId.HasValue)
        {
            return await queryable
                .ApplySorting(req.SortFields, req.SortDirection)
                .ToPagedResultAsync<Ram, RamDto>(
                    req.PageIndex,
                    req.PageSize,
                    mapper.ConfigurationProvider,
                    cancellationToken);
        }

        Cpu? cpu = null;
        if (filter.CpuId is { } cpuId)
        {
            cpu = await context.Cpus
                .AsNoTracking()
                .Include(x => x.RamCompats)
                .FirstOrDefaultAsync(x => x.Id == cpuId, cancellationToken);

            if (cpu is null)
                return PagedResult<RamDto>.Empty(req);
        }

        Motherboard? motherboard = null;
        if (filter.MotherboardId is { } motherboardId)
        {
            motherboard = await context.Motherboards
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == motherboardId, cancellationToken);

            if (motherboard is null)
                return PagedResult<RamDto>.Empty(req);
        }

        // Domain Check*MemoryCompatibility is not EF-translatable; filter in memory then page.
        IEnumerable<Ram> memories = await queryable.ToListAsync(cancellationToken);

        if (cpu is not null)
        {
            memories = memories.Where(x =>
                cpu.CheckMemoryCompatibility(x).Status != PartsCompatibility.Incompatible);
        }

        if (motherboard is not null)
            memories = memories.Where(motherboard.CheckMemoryCompatibility);

        var list = memories
            .ApplySorting(req.SortFields, req.SortDirection)
            .ToList();

        return new PagedResult<RamDto>
        {
            PageIndex = req.PageIndex,
            PageSize = req.PageSize,
            TotalCount = list.Count,
            Items = mapper.Map<List<RamDto>>(list
                .Skip(req.PageIndex * req.PageSize)
                .Take(req.PageSize))
        };
    }
}
