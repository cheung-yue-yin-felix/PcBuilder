using MediatR;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis.Queries;

public record GetChassisMbFormFactorsByChassisIdQuery(Guid ChassisId)
    : IRequest<List<MbFormFactor>>;
