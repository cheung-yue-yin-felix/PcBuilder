using MediatR;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis.Queries;

public class GetChassisMbFormFactorsByChassisIdHandler(IChassisReadStore store)
    : IRequestHandler<GetChassisMbFormFactorsByChassisIdQuery, List<MbFormFactor>>
{
    public async Task<List<MbFormFactor>> Handle(
        GetChassisMbFormFactorsByChassisIdQuery request,
        CancellationToken cancellationToken)
    {
        return await store.ListMbFormFactorsAsync(request.ChassisId, cancellationToken);
    }
}
