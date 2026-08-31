using MediatR;
using PcBuilderBackend.Application.Catalog.ChassisFans.Dto;

namespace PcBuilderBackend.Application.Catalog.ChassisFans.Queries;

public class GetChassisFanByIdHandler(IChassisFanReadStore store)
    : IRequestHandler<GetChassisFanByIdQuery, ChassisFanDto?>
{
    public Task<ChassisFanDto?> Handle(GetChassisFanByIdQuery request, CancellationToken cancellationToken) =>
        store.GetByIdAsync(request.Id, cancellationToken);
}
