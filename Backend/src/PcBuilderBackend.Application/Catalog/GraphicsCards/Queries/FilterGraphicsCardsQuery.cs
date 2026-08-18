using MediatR;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Queries;

public record FilterGraphicsCardsQuery(PagedRequest<GraphicsCardFilter> Request) : IRequest<PagedResult<GraphicsCardListItemDto>>;