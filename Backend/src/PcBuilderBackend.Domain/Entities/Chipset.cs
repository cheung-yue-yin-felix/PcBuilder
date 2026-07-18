namespace PcBuilderBackend.Domain.Entities;

public class Chipset : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public Guid ManufacturerId { get; set; }
    
    public Guid SocketId { get; set; }

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual Socket Socket { get; set; } = null!;
    
    public virtual ICollection<Motherboard> Motherboards { get; set; } = new List<Motherboard>();
}