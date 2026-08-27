namespace PcBuilderBackend.Domain.Entities;

public class GpuSeries : ProductEntity
{
    private readonly List<Gpu> _gpus = [];
    public IReadOnlyCollection<Gpu> Gpus => _gpus;

    protected GpuSeries()
    {
    }

    public GpuSeries(Guid manufacturerId, string name)
    {
        SetName(name);
        SetManufacturer(manufacturerId);
    }
}
