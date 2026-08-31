using MediatR;
using PcBuilderBackend.Application.Catalog.Psus.Dto;

namespace PcBuilderBackend.Application.Catalog.Psus.Queries;

public class GetPsuByIdHandler(IPsuReadStore store) : IRequestHandler<GetPsuByIdQuery, PsuDto?>
{
    public Task<PsuDto?> Handle(GetPsuByIdQuery request, CancellationToken cancellationToken) =>
        store.GetByIdAsync(request.Id, cancellationToken);
}
