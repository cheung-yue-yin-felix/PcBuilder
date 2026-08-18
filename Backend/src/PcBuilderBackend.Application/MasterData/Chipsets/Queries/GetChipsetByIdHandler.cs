using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Chipsets.Dto;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Queries;

public class GetChipsetByIdHandler(IApplicationDbContext context, IMapper mapper, ICacheService cache)
    : IRequestHandler<GetChipsetByIdQuery, ChipsetDto?>
{
    public async Task<ChipsetDto?> Handle(GetChipsetByIdQuery request, CancellationToken cancellationToken)
    {
        var key = MasterDataCacheKeys.Chipsets.ById(request.Id);
        var cached = await cache.GetAsync<ChipsetDto>(key, cancellationToken);
        if (cached is not null)
            return cached;

        var chipset = await context.Chipsets.AsNoTracking()
            .Include(c => c.Manufacturer)
            .Include(c => c.Socket)
            .FirstOrDefaultAsync(c => c.Id == request.Id && c.IsActive, cancellationToken);

        if (chipset is null)
            return null;

        var dto = mapper.Map<ChipsetDto>(chipset);
        await cache.SetAsync(key, dto, MasterDataCacheKeys.DefaultTtl, cancellationToken);
        return dto;
    }
}
