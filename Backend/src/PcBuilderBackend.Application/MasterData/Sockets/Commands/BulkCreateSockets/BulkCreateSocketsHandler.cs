using AutoMapper;
using MediatR;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Sockets.Dto;

namespace PcBuilderBackend.Application.MasterData.Sockets.Commands.BulkCreateSockets;

public class BulkCreateSocketsHandler(IApplicationDbContext context, IMapper mapper): IRequestHandler<BulkCreateSocketsCommand, List<SocketDto>>
{
    public async Task<List<SocketDto>> Handle(BulkCreateSocketsCommand request, CancellationToken cancellationToken)
    {
        var result = new List<SocketDto>();

        foreach (var entity in request.Sockets.Select(x => new Domain.Entities.Socket(x.ManufacturerId, x.Name)))
        {
            context.Sockets.Add(entity);
            result.Add(mapper.Map<SocketDto>(entity));
        }
        
        await context.SaveChangesAsync(cancellationToken);
        return result;
    }
}