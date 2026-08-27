using MediatR;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;

namespace PcBuilderBackend.Application.Catalog.Cpus.Queries;

public class GetCpuByIdHandler(ICpuReadStore store) : IRequestHandler<GetCpuByIdQuery, CpuDto?>
{
    public Task<CpuDto?> Handle(GetCpuByIdQuery request, CancellationToken cancellationToken) =>
        store.GetByIdAsync(request.Id, cancellationToken);
}
