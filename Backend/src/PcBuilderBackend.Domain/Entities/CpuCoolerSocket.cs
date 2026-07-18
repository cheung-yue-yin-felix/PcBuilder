namespace PcBuilderBackend.Domain.Entities;

public class CpuCoolerSocket : BaseEntity
{
    public Guid CpuCoolerId { get; set; }
    
    public Guid SocketId { get; set; }
    
    public virtual CpuCooler CpuCooler { get; set; } = null!;
    
    public virtual Socket Socket { get; set; } = null!;
}