namespace PcBuilderBackend.Application.Catalog.Cpus;

public interface ICpuFields
{
    string Name { get; }
    Guid ManufacturerId { get; }
    Guid SocketId { get; }
    Guid SeriesId { get; }
    int MaxMemoryGb { get; }
    int ThermalDesignPower { get; }
    int PowerConsumptionWatts { get; }
}
