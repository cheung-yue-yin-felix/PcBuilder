using MediatR;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Commands.DeleteMotherboard;

public record DeleteMotherboardCommand(Guid Id) : IRequest<bool>;
