using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Manufacturers.Dto;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Queries;

public class GetManufacturerByIdHandler(IApplicationDbContext context, IMapper mapper, ICacheService cache)
    : IRequestHandler<GetManufacturerByIdQuery, ManufacturerDto?>
{
    public async Task<ManufacturerDto?> Handle(GetManufacturerByIdQuery request, CancellationToken cancellationToken)
    {
        var key = MasterDataCacheKeys.Manufacturers.ById(request.Id);
        var cached = await cache.GetAsync<ManufacturerDto>(key, cancellationToken);
        if (cached is not null)
            return cached;

        var entity = await context.Manufacturers.AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == request.Id && m.IsActive, cancellationToken);

        if (entity is null)
            return null;

        var dto = mapper.Map<ManufacturerDto>(entity);
        await cache.SetAsync(key, dto, MasterDataCacheKeys.DefaultTtl, cancellationToken);
        return dto;
    }
}
