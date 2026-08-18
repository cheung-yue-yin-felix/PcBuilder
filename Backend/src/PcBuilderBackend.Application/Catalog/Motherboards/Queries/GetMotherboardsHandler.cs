using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Queries;

public class GetMotherboardsHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetMotherboardsQuery, PagedResult<MotherboardListItemDto>>
{
    public async Task<PagedResult<MotherboardListItemDto>> Handle(GetMotherboardsQuery request, CancellationToken cancellationToken)
    {
        return await context.Motherboards
            .AsNoTracking()
            .Include(x => x.Manufacturer)
            .Include(x => x.Socket)
            .Include(x => x.Chipset)
            .ApplySorting(request.Request.SortFields, request.Request.SortDirection)
            .ToPagedResultAsync<Motherboard, MotherboardListItemDto>(
                request.Request.PageIndex,
                request.Request.PageSize,
                mapper.ConfigurationProvider,
                cancellationToken);
    }
}
