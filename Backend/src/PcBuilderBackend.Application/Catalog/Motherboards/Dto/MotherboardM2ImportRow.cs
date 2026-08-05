using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Dto;

public class MotherboardM2ImportRow
{
    public int ParentRowNumber { get; init; }
    public PcieGeneration PcieGeneration { get; init; }
    public int SlotCount { get; init; }
    public List<M2FormFactor> FormFactors { get; init; } = [];
}
