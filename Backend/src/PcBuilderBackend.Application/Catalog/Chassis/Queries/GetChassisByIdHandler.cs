using MediatR;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;

namespace PcBuilderBackend.Application.Catalog.Chassis.Queries;

public class GetChassisByIdHandler(IChassisReadStore store)
    : IRequestHandler<GetChassisByIdQuery, ChassisDto?>
{
    public async Task<ChassisDto?> Handle(GetChassisByIdQuery request, CancellationToken cancellationToken)
    {
        return await store.GetByIdAsync(request.Id, cancellationToken);
    }
}