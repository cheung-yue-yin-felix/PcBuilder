using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Chassis.Queries;

public class FilterChassisHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<FilterChassisQuery, PagedResult<ChassisListItemDto>>
{
    public async Task<PagedResult<ChassisListItemDto>> Handle(FilterChassisQuery query, CancellationToken cancellationToken)
    {
        return await context.Chassis
            .AsNoTracking()
            .Include(x => x.MbFormFactors)
            .WhereIf(!string.IsNullOrWhiteSpace(query.Request.Filter.Name), x => x.Name.Contains(query.Request.Filter.Name!))
            .WhereIf(query.Request.Filter.ManufacturerId.HasValue, x => x.ManufacturerId == query.Request.Filter.ManufacturerId)
            .WhereIf(query.Request.Filter.SupportedMbFormFactors.Count != 0, x => x.MbFormFactors.Any(f => query.Request.Filter.SupportedMbFormFactors.Contains(f.MbFormFactor)))
            .WhereIf(query.Request.Filter.HeightMm != null, x => x.HeightMm <= query.Request.Filter.HeightMm!.Max && x.HeightMm >= query.Request.Filter.HeightMm!.Min)
            .WhereIf(query.Request.Filter.LengthMm != null, x => x.LengthMm <= query.Request.Filter.LengthMm!.Max && x.LengthMm >= query.Request.Filter.LengthMm!.Min)
            .WhereIf(query.Request.Filter.WidthMm != null, x => x.WidthMm <= query.Request.Filter.WidthMm!.Max && x.WidthMm >= query.Request.Filter.WidthMm!.Min)
            .WhereIf(query.Request.Filter.MotherboardMaxWidthMm != null, x => x.MotherboardMaxWidthMm <= query.Request.Filter.MotherboardMaxWidthMm!.Max && x.MotherboardMaxWidthMm >= query.Request.Filter.MotherboardMaxWidthMm!.Min)
            .WhereIf(query.Request.Filter.MotherboardMaxHeightMm != null, x => x.MotherboardMaxHeightMm <= query.Request.Filter.MotherboardMaxHeightMm!.Max && x.MotherboardMaxHeightMm >= query.Request.Filter.MotherboardMaxHeightMm!.Min)
            .WhereIf(query.Request.Filter.MaxCpuCoolerHeightMm != null, x => x.MaxCpuCoolerHeightMm <= query.Request.Filter.MaxCpuCoolerHeightMm!.Max && x.MaxCpuCoolerHeightMm >= query.Request.Filter.MaxCpuCoolerHeightMm!.Min)
            .WhereIf(query.Request.Filter.MaxGraphicsCardLengthMm != null, x => x.MaxGraphicsCardLengthMm <= query.Request.Filter.MaxGraphicsCardLengthMm!.Max && x.MaxGraphicsCardLengthMm >= query.Request.Filter.MaxGraphicsCardLengthMm!.Min)
            .WhereIf(query.Request.Filter.MaxPsuLengthMm != null, x => x.MaxPsuLengthMm <= query.Request.Filter.MaxPsuLengthMm!.Max && x.MaxPsuLengthMm >= query.Request.Filter.MaxPsuLengthMm!.Min)
            .ApplySorting(query.Request.SortFields, query.Request.SortDirection)
            .ToPagedResultAsync<Domain.Entities.Chassis, ChassisListItemDto>(
                query.Request.PageIndex,
                query.Request.PageSize,
                mapper.ConfigurationProvider,
                cancellationToken);
    }
}