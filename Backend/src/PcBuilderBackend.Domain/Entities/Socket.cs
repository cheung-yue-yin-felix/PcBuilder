namespace PcBuilderBackend.Domain.Entities;

public class Socket : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public Guid ManufacturerId { get; set; }

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual ICollection<Motherboard> Motherboards { get; set; } = new List<Motherboard>();

    public virtual ICollection<Cpu> Cpus { get; set; } = new List<Cpu>();
}