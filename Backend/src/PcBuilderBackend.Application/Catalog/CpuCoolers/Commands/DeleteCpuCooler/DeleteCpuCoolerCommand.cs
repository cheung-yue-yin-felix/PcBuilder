using MediatR;

namespace PcBuilderBackend.Application.Catalog.CpuCoolers.Commands.DeleteCpuCooler;

public record DeleteCpuCoolerCommand(Guid Id) : IRequest<bool>;
