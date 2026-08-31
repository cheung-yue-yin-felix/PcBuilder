using MediatR;
using PcBuilderBackend.Application.Catalog.ChassisFans.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.ChassisFans.Queries;

public class FilterChassisFansHandler(IChassisFanReadStore store)
    : IRequestHandler<FilterChassisFansQuery, PagedResult<ChassisFanDto>>
{
    public Task<PagedResult<ChassisFanDto>> Handle(
        FilterChassisFansQuery query,
        CancellationToken cancellationToken) =>
        store.FilterAsync(query.Request, cancellationToken);
}
