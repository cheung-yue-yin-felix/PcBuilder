using MediatR;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Commands.BulkDeleteMotherboards;

public record BulkDeleteMotherboardsCommand(List<Guid> MotherboardIds) : IRequest<bool>;