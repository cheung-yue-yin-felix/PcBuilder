using MediatR;

namespace PcBuilderBackend.Application.Catalog.ChassisFans.Commands.DeleteChassisFan;

public record DeleteChassisFanCommand(Guid Id) : IRequest<bool>;
