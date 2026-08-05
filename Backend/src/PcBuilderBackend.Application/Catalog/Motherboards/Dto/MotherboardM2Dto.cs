using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Dto;

public record MotherboardM2Dto
{
    public PcieGeneration PcieGeneration { get; init; }
    public int SlotCount { get; init; }
    public List<M2FormFactor> FormFactors { get; init; } = [];
}
