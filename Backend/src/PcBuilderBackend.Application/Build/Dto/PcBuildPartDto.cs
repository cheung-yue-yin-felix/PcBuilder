using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Build.Dto;

public record PcBuildPartDto(
    PcBuildPartType Type,
    Guid PartId,
    int Quantity
);