using MediatR;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkUpdateChassisPcieSlots;

public record BulkUpdateChassisPcieSlotsCommand(
    Guid ChassisId,
    List<ChassisPcieSlotDto> PcieSlots) : IRequest<List<ChassisPcieSlotDto>?>;
