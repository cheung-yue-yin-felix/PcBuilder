using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Sockets.Dto;

namespace PcBuilderBackend.Application.MasterData.Sockets.Commands.UpdateSocket;

public class UpdateSocketHandler(IApplicationDbContext context, IMapper mapper, ICacheService cache)
    : IRequestHandler<UpdateSocketCommand, SocketDto?>
{
    public async Task<SocketDto?> Handle(UpdateSocketCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Sockets.FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, cancellationToken);
        if (entity is null) return null;

        entity.Rename(request.Name);
        entity.UpdateManufacturer(request.ManufacturerId);

        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Sockets.Prefix, cancellationToken);
        return mapper.Map<SocketDto>(entity);
    }
}
