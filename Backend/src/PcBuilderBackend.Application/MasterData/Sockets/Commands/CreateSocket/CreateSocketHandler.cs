using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.MasterData.Sockets.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Sockets.Commands.CreateSocket;

public class CreateSocketHandler(
    IRepository<Socket> sockets,
    ILogger<CreateSocketHandler> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ICacheService cache
)
    : IRequestHandler<CreateSocketCommand, SocketDto>
{
    public async Task<SocketDto> Handle(CreateSocketCommand request, CancellationToken cancellationToken)
    {
        var entity = new Socket(request.ManufacturerId, request.Name);
        sockets.Add(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.Created(logger, EntityLog.Socket, entity.Id);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Sockets.Prefix, cancellationToken);
        return mapper.Map<SocketDto>(entity);
    }
}
