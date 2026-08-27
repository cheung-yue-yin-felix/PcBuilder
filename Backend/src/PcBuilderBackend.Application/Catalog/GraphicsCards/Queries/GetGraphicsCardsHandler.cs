using MediatR;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Queries;

public class GetGraphicsCardsHandler(IGraphicsCardReadStore store)
    : IRequestHandler<GetGraphicsCardsQuery, PagedResult<GraphicsCardListItemDto>>
{
    public async Task<PagedResult<GraphicsCardListItemDto>> Handle(
        GetGraphicsCardsQuery query,
        CancellationToken cancellationToken)
    {
        return await store.ListAsync(query.Request, cancellationToken);
    }
}
