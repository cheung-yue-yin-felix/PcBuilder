using MediatR;
using PcBuilderBackend.Application.Catalog.Memories.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.Memories.Queries;

public class FilterMemoriesHandler(IRamReadStore ramReadStore)
    : IRequestHandler<FilterMemoriesQuery, PagedResult<RamDto>>
{
    public async Task<PagedResult<RamDto>> Handle(
        FilterMemoriesQuery query,
        CancellationToken cancellationToken)
    {
        return await ramReadStore.FilterAsync(query.Request, cancellationToken);
    }
}
