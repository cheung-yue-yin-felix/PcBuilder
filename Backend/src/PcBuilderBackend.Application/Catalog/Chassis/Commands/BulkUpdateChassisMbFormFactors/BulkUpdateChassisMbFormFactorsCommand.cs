using MediatR;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkUpdateChassisMbFormFactors;

public record BulkUpdateChassisMbFormFactorsCommand(
    Guid ChassisId,
    List<MbFormFactor> MbFormFactors) : IRequest<List<MbFormFactor>?>;
