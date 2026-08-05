using MediatR;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.UpdateGraphicsCard;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.BulkUpdateGraphicsCards;

public record BulkUpdateGraphicsCardsCommand(List<UpdateGraphicsCardCommand> Cards)
    : IRequest<List<GraphicsCardDto>?>;
