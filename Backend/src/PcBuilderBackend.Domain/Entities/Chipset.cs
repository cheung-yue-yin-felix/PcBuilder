namespace PcBuilderBackend.Domain.Entities;

public class Chipset : ProductEntity
{
    public Guid SocketId { get; set; }
    public Socket Socket { get; set; } = null!;
    public ICollection<Motherboard> Motherboards { get; set; } = new List<Motherboard>();
    public ICollection<CpuSupportChipset> SupportedCpus { get; set; } = new List<CpuSupportChipset>();
    
    protected Chipset() {}

    public Chipset(string name, Guid manufacturerId, Guid socketId)
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
            throw new ArgumentException("Socket ID must not be empty.");
        
        SocketId = socketId;
    }
}
