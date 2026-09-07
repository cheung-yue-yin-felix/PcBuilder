namespace PcBuilderBackend.Domain.ValueObjects;

public sealed class CpuSpecs
{
    public Guid SocketId { get; init; }
    public Guid SeriesId { get; init; }
    public int MaxMemoryGb { get; init; }
    public bool IntegratedGraphics { get; init; }
    public bool IncludedStockCooler { get; init; }
    public int ThermalDesignPower { get; init; }
    public int PowerConsumptionWatts { get; init; }
}
