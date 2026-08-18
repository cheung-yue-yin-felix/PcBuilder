using MediatR;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkUpdateChassisDriveBays;

public record BulkUpdateChassisDriveBaysCommand(
    Guid ChassisId,
    List<ChassisDriveBayDto> DriveBays) : IRequest<List<ChassisDriveBayDto>?>;
