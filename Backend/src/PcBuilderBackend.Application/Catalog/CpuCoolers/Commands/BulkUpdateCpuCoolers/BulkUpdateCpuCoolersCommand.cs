using MediatR;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Commands.UpdateCpuCooler;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Dto;

namespace PcBuilderBackend.Application.Catalog.CpuCoolers.Commands.BulkUpdateCpuCoolers;

public record BulkUpdateCpuCoolersCommand(List<UpdateCpuCoolerCommand> CpuCoolers) : IRequest<List<CpuCoolerDto>?>;
