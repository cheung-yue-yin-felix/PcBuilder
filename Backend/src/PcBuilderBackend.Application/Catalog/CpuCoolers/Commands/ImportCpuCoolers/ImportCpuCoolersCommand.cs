using MediatR;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Dto;

namespace PcBuilderBackend.Application.Catalog.CpuCoolers.Commands.ImportCpuCoolers;

public record ImportCpuCoolersCommand(Stream Stream) : IRequest<List<CpuCoolerDto>>;
