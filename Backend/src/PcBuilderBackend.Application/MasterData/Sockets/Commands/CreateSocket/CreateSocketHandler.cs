using AutoMapper;
using MediatR;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Sockets.Dto;

namespace PcBuilderBackend.Application.MasterData.Sockets.Commands.CreateSocket;

public class CreateSocketHandler(IApplicationDbContext context, IMapper mapper): IRequestHandler<CreateSocketCommand, SocketDto>
{
    public async Task<SocketDto> Handle(CreateSocketCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.Socket(request.ManufacturerId, request.Name);
        context.Sockets.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return mapper.Map<SocketDto>(entity);
    }
}