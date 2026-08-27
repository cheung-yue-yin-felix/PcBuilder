using MediatR;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.CpuCoolers.Queries;

public class FilterCpuCoolersHandler(ICpuCoolerReadStore store)
    : IRequestHandler<FilterCpuCoolersQuery, PagedResult<CpuCoolerListItemDto>>
{
    public async Task<PagedResult<CpuCoolerListItemDto>> Handle(
        FilterCpuCoolersQuery query,
        CancellationToken cancellationToken)
    {
        return await store.FilterAsync(query.Request, cancellationToken);
    }
}
