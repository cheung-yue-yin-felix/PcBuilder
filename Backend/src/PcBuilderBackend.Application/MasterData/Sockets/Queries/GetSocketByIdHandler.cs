using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Sockets.Dto;

namespace PcBuilderBackend.Application.MasterData.Sockets.Queries;

public class GetSocketByIdHandler(IApplicationDbContext context, IMapper mapper): IRequestHandler<GetSocketByIdQuery, SocketDto?>
{
    public async Task<SocketDto?> Handle(GetSocketByIdQuery request, CancellationToken cancellationToken)
    {
        var socket = await context.Sockets.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, cancellationToken);
        return socket == null ? null : mapper.Map<SocketDto>(socket);
    }
}