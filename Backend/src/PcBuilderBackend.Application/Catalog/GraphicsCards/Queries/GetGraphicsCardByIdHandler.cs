using MediatR;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Queries;

public class GetGraphicsCardByIdHandler(IGraphicsCardReadStore store)
    : IRequestHandler<GetGraphicsCardByIdQuery, GraphicsCardDto?>
{
    public async Task<GraphicsCardDto?> Handle(
        GetGraphicsCardByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await store.GetByIdAsync(request.Id, cancellationToken);
    }
}
