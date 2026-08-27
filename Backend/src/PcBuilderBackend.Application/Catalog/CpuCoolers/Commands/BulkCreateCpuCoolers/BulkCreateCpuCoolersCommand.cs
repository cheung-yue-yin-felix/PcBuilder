using MediatR;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Dto;

namespace PcBuilderBackend.Application.Catalog.CpuCoolers.Commands.BulkCreateCpuCoolers;

public record BulkCreateCpuCoolersCommand(List<CpuCoolerDto> CpuCoolers) : IRequest<List<CpuCoolerDto>>;
