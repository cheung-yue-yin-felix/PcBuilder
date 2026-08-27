using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Queries;

public class GetWirelessNetworkAdapterByIdHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetWirelessNetworkAdapterByIdQuery, WirelessNetworkAdapterDto?>
{
    public async Task<WirelessNetworkAdapterDto?> Handle(
        GetWirelessNetworkAdapterByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await context.WirelessNetworkAdapters
            .AsNoTracking()
            .Include(x => x.Manufacturer)
            .Where(x => x.Id == request.Id && x.IsActive)
            .ProjectTo<WirelessNetworkAdapterDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
