namespace PcBuilderBackend.Domain.Entities;

public class CpuSeries: BaseEntity
{
    public Guid ManufacturerId { get; set; }
    public Guid SocketId { get; set; }
    public string Name { get; set; } = string.Empty;
}