using MediatR;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;

namespace PcBuilderBackend.Application.Catalog.Cpus.Queries;

public class ListCompatibleMemoriesHandler(ICpuReadStore store)
    : IRequestHandler<ListCompatibleMemoriesQuery, List<CpuRamCompatDto>>
{
    public Task<List<CpuRamCompatDto>> Handle(
        ListCompatibleMemoriesQuery request,
        CancellationToken cancellationToken) =>
        store.ListRamCompatsAsync(request.CpuId, cancellationToken);
}
