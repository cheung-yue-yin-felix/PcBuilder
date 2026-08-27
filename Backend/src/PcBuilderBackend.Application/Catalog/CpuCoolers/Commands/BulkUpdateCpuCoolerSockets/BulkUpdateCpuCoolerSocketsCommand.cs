using MediatR;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Dto;

namespace PcBuilderBackend.Application.Catalog.CpuCoolers.Commands.BulkUpdateCpuCoolerSockets;

public record BulkUpdateCpuCoolerSocketsCommand(
    Guid CpuCoolerId,
    List<CpuCoolerSocketDto> Sockets) : IRequest<List<CpuCoolerSocketDto>?>;
