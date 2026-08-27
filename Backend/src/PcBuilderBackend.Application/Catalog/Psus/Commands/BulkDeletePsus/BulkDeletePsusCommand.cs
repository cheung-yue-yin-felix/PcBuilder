using MediatR;

namespace PcBuilderBackend.Application.Catalog.Psus.Commands.BulkDeletePsus;

public record BulkDeletePsusCommand(List<Guid> Ids) : IRequest<bool>;
