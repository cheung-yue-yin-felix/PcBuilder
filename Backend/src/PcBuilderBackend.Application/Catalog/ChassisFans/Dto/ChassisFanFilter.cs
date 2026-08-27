using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.ChassisFans.Dto;

public record ChassisFanFilter(
    string? Name,
    Guid? ManufacturerId,
    FanDiameterMm? DiameterMm,
    int? FansCountPerPack,
    Guid? ChassisId
);