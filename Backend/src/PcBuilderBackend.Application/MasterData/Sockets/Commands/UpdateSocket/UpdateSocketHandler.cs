using AutoMapper;
using MediatR;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Sockets.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Sockets.Commands.UpdateSocket;

public class UpdateSocketHandler(
    IRepository<Socket> sockets,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ICacheService cache)
    : IRequestHandler<UpdateSocketCommand, SocketDto?>
{
    public async Task<SocketDto?> Handle(UpdateSocketCommand request, CancellationToken cancellationToken)
    {
        var entity = await sockets.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null) return null;

        entity.Rename(request.Name);
        entity.UpdateManufacturer(request.ManufacturerId);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Sockets.Prefix, cancellationToken);
        return mapper.Map<SocketDto>(entity);
    }
}
