using MediatR;
using PcBuilderBackend.Application.Catalog.Psus.Dto;

namespace PcBuilderBackend.Application.Catalog.Psus.Queries;

public class GetPsuCablesByPsuIdHandler(IPsuReadStore store)
    : IRequestHandler<GetPsuCablesByPsuIdQuery, List<PsuCableDto>>
{
    public Task<List<PsuCableDto>> Handle(GetPsuCablesByPsuIdQuery request, CancellationToken cancellationToken) =>
        store.ListCablesAsync(request.PsuId, cancellationToken);
}
