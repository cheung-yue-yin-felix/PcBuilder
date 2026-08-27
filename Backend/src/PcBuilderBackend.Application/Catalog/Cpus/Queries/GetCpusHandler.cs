using MediatR;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.Cpus.Queries;

public class GetCpusHandler(ICpuReadStore store)
    : IRequestHandler<GetCpusQuery, PagedResult<CpuListItemDto>>
{
    public Task<PagedResult<CpuListItemDto>> Handle(GetCpusQuery query, CancellationToken cancellationToken) =>
        store.ListAsync(query.Request, cancellationToken);
}
