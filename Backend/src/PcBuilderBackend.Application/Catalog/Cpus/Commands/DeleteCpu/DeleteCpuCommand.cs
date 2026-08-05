using MediatR;

namespace PcBuilderBackend.Application.Catalog.Cpus.Commands.DeleteCpu;

public record DeleteCpuCommand(Guid Id): IRequest<bool>;
