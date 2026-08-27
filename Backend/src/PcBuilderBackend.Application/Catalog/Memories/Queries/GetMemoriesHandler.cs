using MediatR;
using PcBuilderBackend.Application.Catalog.Memories.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.Memories.Queries;

public class GetMemoriesHandler(IRamReadStore store) : IRequestHandler<GetMemoriesQuery, PagedResult<RamDto>>
{
    public async Task<PagedResult<RamDto>> Handle(GetMemoriesQuery query, CancellationToken cancellationToken)
    {
        return await store.ListAsync(query.Request, cancellationToken);
    }
}
