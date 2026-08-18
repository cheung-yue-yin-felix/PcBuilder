using MediatR;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkUpdateChassisPsuFormFactors;

public record BulkUpdateChassisPsuFormFactorsCommand(
    Guid ChassisId,
    List<PsuFormFactor> PsuFormFactors) : IRequest<List<PsuFormFactor>?>;
