using MediatR;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.Cpus.Queries;

public class FilterCpusHandler(ICpuReadStore store)
    : IRequestHandler<FilterCpusQuery, PagedResult<CpuListItemDto>>
{
    public Task<PagedResult<CpuListItemDto>> Handle(
        FilterCpusQuery query, CancellationToken cancellationToken) =>
        store.FilterAsync(query.Request, cancellationToken);
}
