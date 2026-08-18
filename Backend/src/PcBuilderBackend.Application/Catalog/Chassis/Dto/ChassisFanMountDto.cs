using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis.Dto;

public record ChassisFanMountDto(FanMountLocation Location, bool SingleDiameterOnly, List<ChassisFanMountOptionDto> Options);
