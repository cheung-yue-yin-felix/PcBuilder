using MediatR;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkDeleteChassis;

public record BulkDeleteChassisCommand(List<Guid> Ids) : IRequest<bool>;
