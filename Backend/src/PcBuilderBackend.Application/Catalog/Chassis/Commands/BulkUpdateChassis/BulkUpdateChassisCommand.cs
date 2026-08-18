using MediatR;
using PcBuilderBackend.Application.Catalog.Chassis.Commands.UpdateChassis;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkUpdateChassis;

public record BulkUpdateChassisCommand(List<UpdateChassisCommand> Items)
    : IRequest<List<ChassisDto>?>;
