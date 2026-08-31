using MediatR;
using PcBuilderBackend.Application.Catalog.ChassisFans.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.ChassisFans.Queries;

public class GetChassisFansHandler(IChassisFanReadStore store)
    : IRequestHandler<GetChassisFansQuery, PagedResult<ChassisFanDto>>
{
    public Task<PagedResult<ChassisFanDto>> Handle(GetChassisFansQuery query, CancellationToken cancellationToken) =>
        store.ListAsync(query.Request, cancellationToken);
}
