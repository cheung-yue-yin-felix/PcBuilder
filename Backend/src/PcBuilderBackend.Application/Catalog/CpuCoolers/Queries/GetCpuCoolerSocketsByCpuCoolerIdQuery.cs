using MediatR;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Dto;

namespace PcBuilderBackend.Application.Catalog.CpuCoolers.Queries;

public record GetCpuCoolerSocketsByCpuCoolerIdQuery(Guid CpuCoolerId)
    : IRequest<List<CpuCoolerSocketDto>>;
