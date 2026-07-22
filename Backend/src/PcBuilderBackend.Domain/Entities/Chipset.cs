namespace PcBuilderBackend.Domain.Entities;

public class Chipset : ProductEntity
{
    public Guid SocketId { get; set; }
    public virtual Socket Socket { get; set; } = null!;
    public virtual ICollection<Motherboard> Motherboards { get; set; } = new List<Motherboard>();
}