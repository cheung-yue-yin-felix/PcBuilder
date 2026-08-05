using MediatR;
using PcBuilderBackend.Application.Catalog.Memories.Dto;

namespace PcBuilderBackend.Application.Catalog.Memories.Queries;

public record GetMemoriesByCpuIdQuery(Guid CpuId) : IRequest<List<RamDto>>;
