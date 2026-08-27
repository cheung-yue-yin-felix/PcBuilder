using MediatR;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.CpuCoolers.Queries;

public class GetCpuCoolersHandler(ICpuCoolerReadStore store)
    : IRequestHandler<GetCpuCoolersQuery, PagedResult<CpuCoolerListItemDto>>
{
    public async Task<PagedResult<CpuCoolerListItemDto>> Handle(
        GetCpuCoolersQuery query,
        CancellationToken cancellationToken)
    {
        return await store.ListAsync(query.Request, cancellationToken);
    }
}
