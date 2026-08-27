using MediatR;

namespace PcBuilderBackend.Application.Catalog.CpuCoolers.Commands.BulkDeleteCpuCoolers;

public record BulkDeleteCpuCoolersCommand(List<Guid> CpuCoolerIds) : IRequest<bool>;
