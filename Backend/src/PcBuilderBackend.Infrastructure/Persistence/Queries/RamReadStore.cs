using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Memories;
using PcBuilderBackend.Application.Catalog.Memories.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Infrastructure.Persistence.Queries;

public class RamReadStore(PcBuilderDbContext db, IMapper mapper) : IRamReadStore
{
    public async Task<RamDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await db.Rams
            .AsNoTracking()
            .Include(x => x.Manufacturer)
            .FirstOrDefaultAsync(r => r.Id == id && r.IsActive, cancellationToken);
        return entity == null ? null : mapper.Map<RamDto>(entity);
    }

    public async Task<PagedResult<RamDto>> ListAsync(PagedRequest request, CancellationToken cancellationToken)
    {
        return await db.Rams
            .AsNoTracking()
            .Include(x => x.Manufacturer)
            .ApplySorting(request.SortFields, request.SortDirection)
            .ToPagedResultAsync<Ram, RamDto>(
                request.PageIndex,
                request.PageSize,
                mapper.ConfigurationProvider,
                cancellationToken);
    }

    public async Task<PagedResult<RamDto>> FilterAsync(PagedRequest<RamFilter> request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        var queryable = db.Rams.AsNoTracking()
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
                .ApplySorting(request.SortFields, request.SortDirection)
                .ToPagedResultAsync<Ram, RamDto>(
                    request.PageIndex,
                    request.PageSize,
                    mapper.ConfigurationProvider,
                    cancellationToken);
        }

        Cpu? cpu = null;
        if (filter.CpuId is { } cpuId)
        {
            cpu = await db.Cpus
                .AsNoTracking()
                .Include(x => x.RamCompats)
                .FirstOrDefaultAsync(x => x.Id == cpuId, cancellationToken);

            if (cpu is null)
                return PagedResult<RamDto>.Empty(request);
        }

        Motherboard? motherboard = null;
        if (filter.MotherboardId is { } motherboardId)
        {
            motherboard = await db.Motherboards
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == motherboardId, cancellationToken);

            if (motherboard is null)
                return PagedResult<RamDto>.Empty(request);
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
            .ApplySorting(request.SortFields, request.SortDirection)
            .ToList();

        return new PagedResult<RamDto>
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalCount = list.Count,
            Items = mapper.Map<List<RamDto>>(list
                .Skip(request.PageIndex * request.PageSize)
                .Take(request.PageSize))
        };
    }
}