namespace PcBuilderBackend.Domain.Entities;

public class CpuSeries: ProductEntity
{
    public Guid SocketId { get; set; }
    public Socket Socket { get; set; } = null!;
    public ICollection<Cpu> Cpu { get; set; } = new List<Cpu>();

    protected CpuSeries() {}

    public CpuSeries(Guid manufacturerId, Guid socketId, string name)
    {
        SetName(name);
        SetManufacturer(manufacturerId);
        SetSpecs(socketId);
    }

    public void UpdateSpecs(Guid socketId)
    {
        SetSpecs(socketId);
        UpdatedAtUtc = DateTime.UtcNow;
    }
    
    private void SetSpecs(Guid socketId)
    {
        if (socketId == Guid.Empty)
            throw new ArgumentException("Socket ID cannot be empty.");
        
        SocketId = socketId;
    }
}
