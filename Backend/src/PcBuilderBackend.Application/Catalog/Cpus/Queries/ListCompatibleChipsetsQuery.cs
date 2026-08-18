using MediatR;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;

namespace PcBuilderBackend.Application.Catalog.Cpus.Queries;

public record ListCompatibleChipsetsQuery(Guid CpuId) : IRequest<List<CpuSupportChipsetDto>>;
