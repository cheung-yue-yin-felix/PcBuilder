namespace PcBuilderBackend.Domain.Entities;

public class GpuSeries: ProductEntity
{
    public ICollection<Gpu> Gpus { get; set; } = [];
    
    protected GpuSeries() {}

    public GpuSeries(Guid manufacturerId, string name)
    {
        SetName(name);
        SetManufacturer(manufacturerId);
    }
}
