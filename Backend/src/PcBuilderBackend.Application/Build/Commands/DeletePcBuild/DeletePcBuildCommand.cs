using MediatR;

namespace PcBuilderBackend.Application.Build.Commands.DeletePcBuild;

public record DeletePcBuildCommand(Guid Id) : IRequest<bool>;