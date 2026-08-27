using MediatR;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;

namespace PcBuilderBackend.Application.Catalog.Cpus.Queries;

public class ListCompatibleChipsetsHandler(ICpuReadStore store)
    : IRequestHandler<ListCompatibleChipsetsQuery, List<CpuSupportChipsetDto>>
{
    public Task<List<CpuSupportChipsetDto>> Handle(
        ListCompatibleChipsetsQuery request,
        CancellationToken cancellationToken) =>
        store.ListSupportChipsetsAsync(request.CpuId, cancellationToken);
}
