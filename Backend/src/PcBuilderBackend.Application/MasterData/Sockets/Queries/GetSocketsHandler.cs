using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Sockets.Dto;

namespace PcBuilderBackend.Application.MasterData.Sockets.Queries;

public class GetSocketsHandler(IApplicationDbContext context, IMapper mapper, ICacheService cache)
    : IRequestHandler<GetSocketsQuery, List<SocketDto>>
{
    public Task<List<SocketDto>> Handle(GetSocketsQuery request, CancellationToken cancellationToken)
    {
        return cache.GetOrSetAsync(
            MasterDataCacheKeys.Sockets.All(),
            ct => context.Sockets
                .AsNoTracking()
                .Include(x => x.Manufacturer)
                .Where(x => x.IsActive)
                .ProjectTo<SocketDto>(mapper.ConfigurationProvider)
                .ToListAsync(ct),
            MasterDataCacheKeys.DefaultTtl,
            cancellationToken);
    }
}
