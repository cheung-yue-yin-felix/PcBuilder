using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Queries;

public class GetWirelessNetworkAdaptersHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetWirelessNetworkAdaptersQuery, PagedResult<WirelessNetworkAdapterDto>>
{
    public async Task<PagedResult<WirelessNetworkAdapterDto>> Handle(
        GetWirelessNetworkAdaptersQuery query,
        CancellationToken cancellationToken)
    {
        return await context.WirelessNetworkAdapters
            .AsNoTracking()
            .Include(x => x.Manufacturer)
            .ApplySorting(query.Request.SortFields, query.Request.SortDirection)
            .ToPagedResultAsync<WirelessNetworkAdapter, WirelessNetworkAdapterDto>(
                query.Request.PageIndex,
                query.Request.PageSize,
                mapper.ConfigurationProvider,
                cancellationToken);
    }
}
