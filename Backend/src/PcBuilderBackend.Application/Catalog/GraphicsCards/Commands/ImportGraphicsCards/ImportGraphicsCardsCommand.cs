using MediatR;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.ImportGraphicsCards;

public record ImportGraphicsCardsCommand(Stream Stream) : IRequest<List<GraphicsCardDto>>;
