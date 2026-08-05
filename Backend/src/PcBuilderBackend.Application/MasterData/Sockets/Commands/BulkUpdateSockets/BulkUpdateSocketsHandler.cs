using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Sockets.Dto;

namespace PcBuilderBackend.Application.MasterData.Sockets.Commands.BulkUpdateSockets;

public class BulkUpdateSocketsHandler(IApplicationDbContext context, IMapper mapper): IRequestHandler<BulkUpdateSocketsCommand, List<SocketDto>>
{
    public async Task<List<SocketDto>> Handle(BulkUpdateSocketsCommand request, CancellationToken cancellationToken)
    {
        var result = new List<SocketDto>();

        foreach (var socket in request.Sockets)
        {
            var entity = await context.Sockets.FirstOrDefaultAsync(x => x.Id == socket.Id && x.IsActive, cancellationToken);
            if (entity is null) return result;
            entity.Rename(socket.Name);
            entity.UpdateManufacturer(socket.ManufacturerId);
            result.Add(mapper.Map<SocketDto>(entity));
        }

        await context.SaveChangesAsync(cancellationToken);
        return result;
    }
}