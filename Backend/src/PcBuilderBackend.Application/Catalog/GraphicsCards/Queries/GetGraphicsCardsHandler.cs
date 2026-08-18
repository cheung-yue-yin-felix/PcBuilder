using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Queries;

public class GetGraphicsCardsHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetGraphicsCardsQuery, PagedResult<GraphicsCardListItemDto>>
{
    public async Task<PagedResult<GraphicsCardListItemDto>> Handle(
        GetGraphicsCardsQuery request,
        CancellationToken cancellationToken)
    {
        return await context.GraphicsCards
            .AsNoTracking()
            .Include(x => x.Gpu)
            .ThenInclude(x => x.Series)
            .ApplySorting(request.Request.SortFields, request.Request.SortDirection)
            .ToPagedResultAsync<GraphicsCard, GraphicsCardListItemDto>(
                request.Request.PageIndex,
                request.Request.PageSize,
                mapper.ConfigurationProvider,
                cancellationToken);
    }
}
