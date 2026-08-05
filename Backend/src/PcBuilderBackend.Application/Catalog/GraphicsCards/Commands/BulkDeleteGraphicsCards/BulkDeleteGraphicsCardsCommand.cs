using MediatR;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.BulkDeleteGraphicsCards;

public record BulkDeleteGraphicsCardsCommand(List<Guid> Ids) : IRequest<bool>;
