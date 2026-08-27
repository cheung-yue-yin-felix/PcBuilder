using MediatR;

namespace PcBuilderBackend.Application.Catalog.ChassisFans.Commands.BulkDeleteChassisFans;

public record BulkDeleteChassisFansCommand(List<Guid> Ids) : IRequest<bool>;
