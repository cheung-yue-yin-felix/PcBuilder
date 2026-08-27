using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards;

public interface IGraphicsCardReadStore
{
    Task<GraphicsCardDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PagedResult<GraphicsCardListItemDto>> ListAsync(PagedRequest request,
        CancellationToken cancellationToken);
    Task<PagedResult<GraphicsCardListItemDto>> FilterAsync(PagedRequest<GraphicsCardFilter> request, CancellationToken cancellationToken);
}