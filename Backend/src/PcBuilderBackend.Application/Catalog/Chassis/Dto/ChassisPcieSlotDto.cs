using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis.Dto;

public record ChassisPcieSlotDto(bool LowProfileSlots, int SlotCount, PcieOrientation Orientation);