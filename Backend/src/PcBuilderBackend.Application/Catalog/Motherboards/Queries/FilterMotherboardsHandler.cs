using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Queries;

public class FilterMotherboardsHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<FilterMotherboardsQuery, PagedResult<MotherboardListItemDto>>
{
    public async Task<PagedResult<MotherboardListItemDto>> Handle(
        FilterMotherboardsQuery query,
        CancellationToken cancellationToken)
    {
        var filter = query.Request.Filter;
        var req = query.Request;

        var queryable = context.Motherboards
            .AsNoTracking()
            .WhereIf(!string.IsNullOrWhiteSpace(filter.Name),
                x => x.Name.Contains(filter.Name!))
            .WhereIf(filter.ManufacturerId.HasValue,
                x => x.ManufacturerId == filter.ManufacturerId)
            .WhereIf(filter.SocketId.HasValue,
                x => x.SocketId == filter.SocketId)
            .WhereIf(filter.ChipsetId.HasValue,
                x => x.ChipsetId == filter.ChipsetId)
            .WhereIf(filter.RamSlots.HasValue,
                x => x.RamSlots == filter.RamSlots)
            .WhereIf(filter.MaxMemoryGb.HasValue,
                x => x.MaxMemoryGb == filter.MaxMemoryGb)
            .WhereIf(filter.MaxDimmSizeGb.HasValue,
                x => x.MaxDimmSizeGb == filter.MaxDimmSizeGb)
            .WhereIf(filter.SataPorts.HasValue,
                x => x.SataPorts == filter.SataPorts)
            .WhereIf(filter.FanConnectors.HasValue,
                x => x.FanConnectors == filter.FanConnectors)
            .WhereIf(filter.EpsConnectors.HasValue,
                x => x.EpsConnectors == filter.EpsConnectors)
            .WhereIf(filter.WidthMm?.Min is not null,
                x => x.WidthMm >= filter.WidthMm!.Min)
            .WhereIf(filter.WidthMm?.Max is not null,
                x => x.WidthMm <= filter.WidthMm!.Max)
            .WhereIf(filter.HeightMm?.Min is not null,
                x => x.HeightMm >= filter.HeightMm!.Min)
            .WhereIf(filter.HeightMm?.Max is not null,
                x => x.HeightMm <= filter.HeightMm!.Max)
            .WhereIf(filter.DdrGeneration.HasValue,
                x => x.DdrGeneration == filter.DdrGeneration)
            .WhereIf(filter.RamFormFactor.HasValue,
                x => x.RamFormFactor == filter.RamFormFactor)
            .WhereIf(filter.FormFactor.HasValue,
                x => x.FormFactor == filter.FormFactor)
            .WhereIf(filter.WifiEnabled.HasValue,
                x => x.WifiEnabled == filter.WifiEnabled)
            .WhereIf(filter.BluetoothEnabled.HasValue,
                x => x.BluetoothEnabled == filter.BluetoothEnabled);

        if (filter.ChassisId is not { } chassisId)
        {
            return await queryable
                .ApplySorting(req.SortFields, req.SortDirection)
                .ToPagedResultAsync<Motherboard, MotherboardListItemDto>(
                    req.PageIndex,
                    req.PageSize,
                    mapper.ConfigurationProvider,
                    cancellationToken);
        }

        var chassis = await context.Chassis
            .AsNoTracking()
            .Include(x => x.MbFormFactors)
            .FirstOrDefaultAsync(x => x.Id == chassisId, cancellationToken);

        if (chassis is null)
            return PagedResult<MotherboardListItemDto>.Empty(req);

        // Domain CheckMotherboardCompatibility is not EF-translatable; filter in memory then page.
        var list = (await queryable
                .Include(x => x.Manufacturer)
                .Include(x => x.Socket)
                .Include(x => x.Chipset)
                .ToListAsync(cancellationToken))
            .Where(chassis.CheckMotherboardCompatibility)
            .ApplySorting(req.SortFields, req.SortDirection)
            .ToList();

        return new PagedResult<MotherboardListItemDto>
        {
            PageIndex = req.PageIndex,
            PageSize = req.PageSize,
            TotalCount = list.Count,
            Items = mapper.Map<List<MotherboardListItemDto>>(list
                .Skip(req.PageIndex * req.PageSize)
                .Take(req.PageSize))
        };
    }
}
