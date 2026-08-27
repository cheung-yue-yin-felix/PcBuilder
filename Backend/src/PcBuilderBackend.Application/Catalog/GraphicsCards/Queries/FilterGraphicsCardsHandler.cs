using MediatR;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Queries;

public class FilterGraphicsCardsHandler(IGraphicsCardReadStore store)
    : IRequestHandler<FilterGraphicsCardsQuery, PagedResult<GraphicsCardListItemDto>>
{
    public async Task<PagedResult<GraphicsCardListItemDto>> Handle(
        FilterGraphicsCardsQuery query,
        CancellationToken cancellationToken)
    {
        return await store.FilterAsync(query.Request, cancellationToken);
    }
}
