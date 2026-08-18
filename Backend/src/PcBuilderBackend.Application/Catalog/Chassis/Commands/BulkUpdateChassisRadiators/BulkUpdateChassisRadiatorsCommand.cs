using MediatR;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkUpdateChassisRadiators;

public record BulkUpdateChassisRadiatorsCommand(
    Guid ChassisId,
    List<ChassisRadiatorDto> Radiators) : IRequest<List<ChassisRadiatorDto>?>;
