using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Queries;

public class GetWiredNetworkAdaptersHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetWiredNetworkAdaptersQuery, PagedResult<WiredNetworkAdapterDto>>
{
    public async Task<PagedResult<WiredNetworkAdapterDto>> Handle(
        GetWiredNetworkAdaptersQuery query,
        CancellationToken cancellationToken)
    {
        return await context.WiredNetworkAdapters
            .AsNoTracking()
            .Include(x => x.Manufacturer)
            .ApplySorting(query.Request.SortFields, query.Request.SortDirection)
            .ToPagedResultAsync<WiredNetworkAdapter, WiredNetworkAdapterDto>(
                query.Request.PageIndex,
                query.Request.PageSize,
                mapper.ConfigurationProvider,
                cancellationToken);
    }
}
