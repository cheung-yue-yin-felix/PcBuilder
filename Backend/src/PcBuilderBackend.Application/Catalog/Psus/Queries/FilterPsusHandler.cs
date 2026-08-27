using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Psus.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Psus.Queries;

public class FilterPsusHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<FilterPsusQuery, PagedResult<PsuListItemDto>>
{
    public async Task<PagedResult<PsuListItemDto>> Handle(
        FilterPsusQuery query,
        CancellationToken cancellationToken)
    {
        var filter = query.Request.Filter;
        var req = query.Request;

        var queryable = context.Psus.AsNoTracking()
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
                .ApplySorting(req.SortFields, req.SortDirection)
                .ToPagedResultAsync<Psu, PsuListItemDto>(
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
                .Include(c => c.PsuFormFactors)
                .FirstOrDefaultAsync(c => c.Id == chassisId, cancellationToken);

            if (chassis is null)
                return PagedResult<PsuListItemDto>.Empty(req);
        }

        Motherboard? motherboard = null;
        if (filter.MotherboardId is { } motherboardId)
        {
            motherboard = await context.Motherboards
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == motherboardId, cancellationToken);

            if (motherboard is null)
                return PagedResult<PsuListItemDto>.Empty(req);
        }

        GraphicsCard? graphicsCard = null;
        if (filter.GraphicsCardId is { } graphicsCardId)
        {
            graphicsCard = await context.GraphicsCards
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == graphicsCardId, cancellationToken);

            if (graphicsCard is null)
                return PagedResult<PsuListItemDto>.Empty(req);
        }

        Cpu? cpu = null;
        if (filter.CpuId is { } cpuId)
        {
            cpu = await context.Cpus
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == cpuId, cancellationToken);

            if (cpu is null)
                return PagedResult<PsuListItemDto>.Empty(req);
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
            .ApplySorting(req.SortFields, req.SortDirection)
            .ToList();

        return new PagedResult<PsuListItemDto>
        {
            PageIndex = req.PageIndex,
            PageSize = req.PageSize,
            TotalCount = list.Count,
            Items = mapper.Map<List<PsuListItemDto>>(list
                .Skip(req.PageIndex * req.PageSize)
                .Take(req.PageSize))
        };
    }
}
