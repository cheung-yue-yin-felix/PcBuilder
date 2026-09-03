using MediatR;

namespace PcBuilderBackend.Application.Build.Commands.BulkDeletePcBuild;

public record BulkDeletePcBuildCommand(List<Guid> Ids) : IRequest<bool>;