namespace PcBuilderBackend.Domain.Entities;

public class GpuSeries: NamedEntity
{
    public virtual ICollection<Gpu> Gpus { get; set; } = [];
}