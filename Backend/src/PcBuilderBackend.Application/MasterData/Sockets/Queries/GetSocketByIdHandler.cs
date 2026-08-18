using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Sockets.Dto;

namespace PcBuilderBackend.Application.MasterData.Sockets.Queries;

public class GetSocketByIdHandler(IApplicationDbContext context, IMapper mapper, ICacheService cache)
    : IRequestHandler<GetSocketByIdQuery, SocketDto?>
{
    public async Task<SocketDto?> Handle(GetSocketByIdQuery request, CancellationToken cancellationToken)
    {
        var key = MasterDataCacheKeys.Sockets.ById(request.Id);
        var cached = await cache.GetAsync<SocketDto>(key, cancellationToken);
        if (cached is not null)
            return cached;

        var socket = await context.Sockets.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, cancellationToken);

        if (socket is null)
            return null;

        var dto = mapper.Map<SocketDto>(socket);
        await cache.SetAsync(key, dto, MasterDataCacheKeys.DefaultTtl, cancellationToken);
        return dto;
    }
}
