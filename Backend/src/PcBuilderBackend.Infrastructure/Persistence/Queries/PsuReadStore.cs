using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Psus;
using PcBuilderBackend.Application.Catalog.Psus.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Infrastructure.Persistence.Queries;

public sealed class PsuReadStore(PcBuilderDbContext db, IMapper mapper) : IPsuReadStore
{
    public async Task<PsuDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await db.Psus
            .AsNoTracking()
            .Include(x => x.Manufacturer)
            .Include(x => x.Cables)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity is null ? null : mapper.Map<PsuDto>(entity);
    }

    public Task<PagedResult<PsuListItemDto>> ListAsync(PagedRequest request, CancellationToken cancellationToken) =>
        db.Psus
            .AsNoTracking()
            .ApplySorting(request.SortFields, request.SortDirection)
            .ToPagedResultAsync<Psu, PsuListItemDto>(
                request.PageIndex,
                request.PageSize,
                mapper.ConfigurationProvider,
                cancellationToken);

    public async Task<PagedResult<PsuListItemDto>> FilterAsync(
        PagedRequest<PsuFilter> request,
        CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        var queryable = db.Psus.AsNoTracking()
            .Include(x => x.Manufacturer)
            .WhereIf(!string.IsNullOrWhiteSpace(filter.Name), x => x.Name.Contains(filter.Name!))
            .WhereIf(filter.ManufacturerId.HasValue, x => x.ManufacturerId == filter.ManufacturerId)
            .WhereIf(filter.Wattage is not null,
                x => x.Wattage >= filter.Wattage!.Min && x.Wattage <= filter.Wattage!.Max)
            .WhereIf(filter.Modularity.HasValue, x => x.Modularity == filter.Modularity)
            .WhereIf(filter.FormFactor.HasValue, x => x.FormFactor == filter.FormFactor)
            .WhereIf(filter.LengthMm is not null,
                x => x.LengthMm >= filter.LengthMm!.Min && x.LengthMm <= filter.LengthMm!.Max)
            .WhereIf(filter.WidthMm is not null,
                x => x.WidthMm >= filter.WidthMm!.Min && x.WidthMm <= filter.WidthMm!.Max)
            .WhereIf(filter.HeightMm is not null,
                x => x.HeightMm >= filter.HeightMm!.Min && x.HeightMm <= filter.HeightMm!.Max);

        var needsCompatibility = filter.ChassisId.HasValue
            || filter.MotherboardId.HasValue
            || filter.GraphicsCardId.HasValue
            || filter.CpuId.HasValue;

        if (!needsCompatibility)
        {
            return await queryable
                .ApplySorting(request.SortFields, request.SortDirection)
                .ToPagedResultAsync<Psu, PsuListItemDto>(
                    request.PageIndex,
                    request.PageSize,
                    mapper.ConfigurationProvider,
                    cancellationToken);
        }

        Chassis? chassis = null;
        if (filter.ChassisId is { } chassisId)
        {
            chassis = await db.Chassis
                .AsNoTracking()
                .Include(c => c.PsuFormFactors)
                .FirstOrDefaultAsync(c => c.Id == chassisId, cancellationToken);

            if (chassis is null)
                return PagedResult<PsuListItemDto>.Empty(request);
        }

        Motherboard? motherboard = null;
        if (filter.MotherboardId is { } motherboardId)
        {
            motherboard = await db.Motherboards
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == motherboardId, cancellationToken);

            if (motherboard is null)
                return PagedResult<PsuListItemDto>.Empty(request);
        }

        GraphicsCard? graphicsCard = null;
        if (filter.GraphicsCardId is { } graphicsCardId)
        {
            graphicsCard = await db.GraphicsCards
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == graphicsCardId, cancellationToken);

            if (graphicsCard is null)
                return PagedResult<PsuListItemDto>.Empty(request);
        }

        Cpu? cpu = null;
        if (filter.CpuId is { } cpuId)
        {
            cpu = await db.Cpus
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == cpuId, cancellationToken);

            if (cpu is null)
                return PagedResult<PsuListItemDto>.Empty(request);
        }

        IEnumerable<Psu> psus = await queryable
            .Include(x => x.Cables)
            .ToListAsync(cancellationToken);

        if (chassis is not null)
            psus = psus.Where(chassis.CheckPsuCompatibility);

        if (motherboard is not null)
        {
            psus = psus.Where(x =>
                x.CheckMotherboardCompatibility(motherboard).Status != PartsCompatibility.Incompatible);
        }

        if (graphicsCard is not null)
        {
            psus = psus.Where(x =>
                x.CheckGraphisCardCompatibility(graphicsCard).Status != PartsCompatibility.Incompatible);
        }

        if (cpu is not null)
        {
            psus = psus.Where(x =>
                x.CheckPowerBudget(cpu, graphicsCard).Status != PartsCompatibility.Incompatible);
        }

        var list = psus
            .ApplySorting(request.SortFields, request.SortDirection)
            .ToList();

        return new PagedResult<PsuListItemDto>
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalCount = list.Count,
            Items = mapper.Map<List<PsuListItemDto>>(list
                .Skip(request.PageIndex * request.PageSize)
                .Take(request.PageSize))
        };
    }

    public async Task<List<PsuCableDto>> ListCablesAsync(Guid psuId, CancellationToken cancellationToken)
    {
        var psu = await db.Psus
            .AsNoTracking()
            .Include(x => x.Cables)
            .FirstOrDefaultAsync(x => x.Id == psuId, cancellationToken);

        if (psu is null)
            return [];

        return mapper.Map<List<PsuCableDto>>(
            psu.Cables.Where(x => x.IsActive).OrderBy(x => x.Type).ToList());
    }
}
