using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis.Dto;

public record ChassisFanMountOptionDto(FanDiameterMm Diameter, int SlotCount
);