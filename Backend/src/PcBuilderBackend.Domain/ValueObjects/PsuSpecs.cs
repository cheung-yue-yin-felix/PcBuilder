using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.ValueObjects;

public sealed class PsuSpecs
{
    public int Wattage { get; init; }
    public PsuModularity Modularity { get; init; }
    public PsuFormFactor FormFactor { get; init; }
    public decimal LengthMm { get; init; }
    public decimal WidthMm { get; init; }
    public decimal HeightMm { get; init; }
}
