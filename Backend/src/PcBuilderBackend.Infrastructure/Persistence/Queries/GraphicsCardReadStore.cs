using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.GraphicsCards;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Infrastructure.Persistence.Queries;

public class GraphicsCardReadStore(PcBuilderDbContext db, IMapper mapper) : IGraphicsCardReadStore
{
    public async Task<GraphicsCardDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await db.GraphicsCards
            .AsNoTracking()
            .Include(x => x.Gpu)
            .ThenInclude(x => x.Series)
            .Where(x => x.Id == id && x.IsActive)
        .ProjectTo<GraphicsCardDto>(mapper.ConfigurationProvider)
        .FirstOrDefaultAsync(cancellationToken);
    
    public async Task<PagedResult<GraphicsCardListItemDto>> ListAsync(PagedRequest request, CancellationToken cancellationToken) =>
        await db.GraphicsCards
            .AsNoTracking()
            .Include(x => x.Gpu)
            .ThenInclude(x => x.Series)
            .ApplySorting(request.SortFields, request.SortDirection)
            .ToPagedResultAsync<GraphicsCard, GraphicsCardListItemDto>(
                request.PageIndex,
                request.PageSize,
                mapper.ConfigurationProvider,
                cancellationToken);

    public async Task<PagedResult<GraphicsCardListItemDto>> FilterAsync(PagedRequest<GraphicsCardFilter> request,
        CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        var queryable = db.GraphicsCards.AsNoTracking()
            .Include(x => x.Gpu)
            .ThenInclude(x => x.Series)
            .WhereIf(!string.IsNullOrWhiteSpace(filter.Name), x => x.Name.Contains(filter.Name!))
            .WhereIf(filter.ManufacturerId.HasValue, x => x.ManufacturerId == filter.ManufacturerId)
            .WhereIf(filter.GpuId.HasValue, x => x.GpuId == filter.GpuId)
            .WhereIf(filter.VideoMemoryGb.HasValue, x => x.VideoMemoryGb == filter.VideoMemoryGb)
            .WhereIf(filter.PcieGeneration.HasValue, x => x.PcieGeneration == filter.PcieGeneration)
            .WhereIf(filter.PcieSlotsUsed.HasValue, x => x.PcieSlotsUsed == filter.PcieSlotsUsed)
            .WhereIf(filter.LengthMm is not null,
                x => x.LengthMm >= filter.LengthMm!.Min && x.LengthMm <= filter.LengthMm!.Max)
            .WhereIf(filter.WidthMm is not null,
                x => x.WidthMm >= filter.WidthMm!.Min && x.WidthMm <= filter.WidthMm!.Max)
            .WhereIf(filter.HeightMm is not null,
                x => x.HeightMm >= filter.HeightMm!.Min && x.HeightMm <= filter.HeightMm!.Max)
            .WhereIf(filter.PowerConsumptionWatts is not null,
                x => x.PowerConsumptionWatts >= filter.PowerConsumptionWatts!.Min &&
                     x.PowerConsumptionWatts <= filter.PowerConsumptionWatts!.Max);

        if (!filter.ChassisId.HasValue && !filter.MotherboardId.HasValue)
        {
            return await queryable
                .ApplySorting(request.SortFields, request.SortDirection)
                .ToPagedResultAsync<GraphicsCard, GraphicsCardListItemDto>(
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
                .Include(c => c.PcieSlots)
                .FirstOrDefaultAsync(c => c.Id == chassisId, cancellationToken);

            if (chassis is null)
                return PagedResult<GraphicsCardListItemDto>.Empty(request);
        }

        Motherboard? motherboard = null;
        if (filter.MotherboardId is { } motherboardId)
        {
            motherboard = await db.Motherboards
                .AsNoTracking()
                .Include(m => m.PcieSlots)
                .FirstOrDefaultAsync(m => m.Id == motherboardId, cancellationToken);

            if (motherboard is null)
                return PagedResult<GraphicsCardListItemDto>.Empty(request);
        }

        // Domain Check*GraphicsCardCompatibility is not EF-translatable; filter in memory then page.
        IEnumerable<GraphicsCard> cards = await queryable.ToListAsync(cancellationToken);

        if (chassis is not null)
            cards = cards.Where(chassis.CheckGraphicsCardCompatibility);

        if (motherboard is not null)
        {
            cards = cards.Where(x =>
                motherboard.CheckGraphicsCardCompatibility(x).Status != PartsCompatibility.Incompatible);
        }

        var list = cards
            .ApplySorting(request.SortFields, request.SortDirection)
            .ToList();

        return new PagedResult<GraphicsCardListItemDto>
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalCount = list.Count,
            Items = mapper.Map<List<GraphicsCardListItemDto>>(list
                .Skip(request.PageIndex * request.PageSize)
                .Take(request.PageSize))
        };
    }
}