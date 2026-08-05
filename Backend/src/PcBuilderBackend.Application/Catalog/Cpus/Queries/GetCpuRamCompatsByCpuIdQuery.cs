using MediatR;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;

namespace PcBuilderBackend.Application.Catalog.Cpus.Queries;

public record GetCpuRamCompatsByCpuIdQuery(Guid CpuId) : IRequest<List<CpuRamCompatDto>>;
