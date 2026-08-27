using MediatR;
using PcBuilderBackend.Application.Catalog.Memories.Dto;

namespace PcBuilderBackend.Application.Catalog.Memories.Queries;

public class GetMemoryByIdHandler(IRamReadStore store) : IRequestHandler<GetMemoryByIdQuery, RamDto?>
{
    public async Task<RamDto?> Handle(GetMemoryByIdQuery request, CancellationToken cancellationToken)
    {
        return await store.GetByIdAsync(request.Id, cancellationToken);
    }
}
