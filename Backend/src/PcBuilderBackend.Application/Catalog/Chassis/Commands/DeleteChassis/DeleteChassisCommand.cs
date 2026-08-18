using MediatR;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.DeleteChassis;

public record DeleteChassisCommand(Guid Id) : IRequest<bool>;
