using MediatR;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.DeleteGraphicsCard;

public record DeleteGraphicsCardCommand(Guid Id) : IRequest<bool>;
