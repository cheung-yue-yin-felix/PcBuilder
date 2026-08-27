using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Queries;

public class GetWiredNetworkAdapterByIdHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetWiredNetworkAdapterByIdQuery, WiredNetworkAdapterDto?>
{
    public async Task<WiredNetworkAdapterDto?> Handle(
        GetWiredNetworkAdapterByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await context.WiredNetworkAdapters
            .AsNoTracking()
            .Include(x => x.Manufacturer)
            .Where(x => x.Id == request.Id && x.IsActive)
            .ProjectTo<WiredNetworkAdapterDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
