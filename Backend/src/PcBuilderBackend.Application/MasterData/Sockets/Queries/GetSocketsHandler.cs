using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Sockets.Dto;

namespace PcBuilderBackend.Application.MasterData.Sockets.Queries;

public class GetSocketsHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetSocketsQuery, List<SocketDto>>
{
    public async Task<List<SocketDto>> Handle(GetSocketsQuery request, CancellationToken cancellationToken)
    {
        return await context.Sockets
            .AsNoTracking()
            .Where(x => x.IsActive)
            .ProjectTo<SocketDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }   
}