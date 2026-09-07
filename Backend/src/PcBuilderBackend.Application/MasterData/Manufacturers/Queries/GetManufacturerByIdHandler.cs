using MediatR;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Manufacturers.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Queries;

public class GetManufacturerByIdHandler(IReadStore<ManufacturerDto> store, ICacheService cache)
    : IRequestHandler<GetManufacturerByIdQuery, ManufacturerDto?>
{
    public async Task<ManufacturerDto?> Handle(GetManufacturerByIdQuery request, CancellationToken cancellationToken)
    {
        var key = MasterDataCacheKeys.Manufacturers.ById(request.Id);
        var cached = await cache.GetAsync<ManufacturerDto>(key, cancellationToken);
        if (cached is not null)
            return cached;

        var dto = await store.GetByIdAsync(request.Id, cancellationToken);

        if (dto is null)
            return null;
        
        await cache.SetAsync(key, dto, MasterDataCacheKeys.DefaultTtl, cancellationToken);
        return dto;
    }
}
