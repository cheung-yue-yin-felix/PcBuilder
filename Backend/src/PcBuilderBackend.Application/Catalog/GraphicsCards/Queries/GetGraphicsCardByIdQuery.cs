using MediatR;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Queries;

public record GetGraphicsCardByIdQuery(Guid Id) : IRequest<GraphicsCardDto?>;
