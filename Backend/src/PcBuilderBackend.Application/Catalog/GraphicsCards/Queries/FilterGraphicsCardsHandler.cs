using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Queries;

public class FilterGraphicsCardsHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<FilterGraphicsCardsQuery, PagedResult<GraphicsCardListItemDto>>
{
    public async Task<PagedResult<GraphicsCardListItemDto>> Handle(
        FilterGraphicsCardsQuery query,
        CancellationToken cancellationToken)
    {
        var filter = query.Request.Filter;
        var req = query.Request;

        var queryable = context.GraphicsCards.AsNoTracking()
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
                .ApplySorting(req.SortFields, req.SortDirection)
                .ToPagedResultAsync<GraphicsCard, GraphicsCardListItemDto>(
                    req.PageIndex,
                    req.PageSize,
                    mapper.ConfigurationProvider,
                    cancellationToken);
        }

        Domain.Entities.Chassis? chassis = null;
        if (filter.ChassisId is { } chassisId)
        {
            chassis = await context.Chassis
                .AsNoTracking()
                .Include(c => c.PcieSlots)
                .FirstOrDefaultAsync(c => c.Id == chassisId, cancellationToken);

            if (chassis is null)
                return PagedResult<GraphicsCardListItemDto>.Empty(req);
        }

        Motherboard? motherboard = null;
        if (filter.MotherboardId is { } motherboardId)
        {
            motherboard = await context.Motherboards
                .AsNoTracking()
                .Include(m => m.PcieSlots)
                .FirstOrDefaultAsync(m => m.Id == motherboardId, cancellationToken);

            if (motherboard is null)
                return PagedResult<GraphicsCardListItemDto>.Empty(req);
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
            .ApplySorting(req.SortFields, req.SortDirection)
            .ToList();

        return new PagedResult<GraphicsCardListItemDto>
        {
            PageIndex = req.PageIndex,
            PageSize = req.PageSize,
            TotalCount = list.Count,
            Items = mapper.Map<List<GraphicsCardListItemDto>>(list
                .Skip(req.PageIndex * req.PageSize)
                .Take(req.PageSize))
        };
    }
}
