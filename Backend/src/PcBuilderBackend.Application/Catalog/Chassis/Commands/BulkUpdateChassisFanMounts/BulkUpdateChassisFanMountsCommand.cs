using MediatR;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkUpdateChassisFanMounts;

public record BulkUpdateChassisFanMountsCommand(
    Guid ChassisId,
    List<ChassisFanMountDto> FanMounts) : IRequest<List<ChassisFanMountDto>?>;
