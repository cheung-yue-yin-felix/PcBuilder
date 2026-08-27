using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.ChassisFans.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.ChassisFans.Queries;

public class FilterChassisFansHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<FilterChassisFansQuery, PagedResult<ChassisFanDto>>
{
    public async Task<PagedResult<ChassisFanDto>> Handle(FilterChassisFansQuery query,
        CancellationToken cancellationToken)
    {
        var filter = query.Request.Filter;
        var req = query.Request;

        var queryable = context.ChassisFans.AsNoTracking()
            .WhereIf(filter.ManufacturerId.HasValue,
                x => x.ManufacturerId == filter.ManufacturerId)
            .WhereIf(!string.IsNullOrEmpty(filter.Name), x => x.Name.Contains(filter.Name!))
            .WhereIf(filter.DiameterMm.HasValue, x => x.DiameterMm == filter.DiameterMm)
            .WhereIf(filter.FansCountPerPack.HasValue,
                x => x.FansCountPerPack == filter.FansCountPerPack);

        if (!filter.ChassisId.HasValue)
        {
            return await queryable
                .Include(x => x.Manufacturer)
                .ApplySorting(req.SortFields, req.SortDirection)
                .ToPagedResultAsync<ChassisFan, ChassisFanDto>(
                    req.PageIndex,
                    req.PageSize,
                    mapper.ConfigurationProvider,
                    cancellationToken
                );
        }

        var chassis = await context.Chassis
            .AsNoTracking()
            .Include(c => c.FanMounts)
            .ThenInclude(m => m.Options)
            .FirstOrDefaultAsync(c => c.Id == filter.ChassisId.Value, cancellationToken);

        if (chassis is null)
            return PagedResult<ChassisFanDto>.Empty(req);

        IEnumerable<ChassisFan> chassisFans = await queryable
            .Include(x => x.Manufacturer)
            .ToListAsync(cancellationToken);

        chassisFans = chassisFans.Where(chassis.CheckFanCompatibility);

        var list = chassisFans.ApplySorting(req.SortFields, req.SortDirection).ToList();

        return new PagedResult<ChassisFanDto>
        {
            Items = mapper.Map<List<ChassisFanDto>>(list
                .Skip(req.PageIndex * req.PageSize)
                .Take(req.PageSize)),
            TotalCount = list.Count,
            PageIndex = req.PageIndex,
            PageSize = req.PageSize
        };
    }
}