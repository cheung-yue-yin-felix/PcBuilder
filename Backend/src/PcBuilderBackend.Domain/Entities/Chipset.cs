namespace PcBuilderBackend.Domain.Entities;

public class Chipset : ProductEntity
{
    public Guid SocketId { get; private set; }
    public Socket Socket { get; private set; } = null!;

    private readonly List<CpuSupportChipset> _supportedCpus = [];
    public IReadOnlyCollection<CpuSupportChipset> SupportedCpus => _supportedCpus;

    private readonly List<Motherboard> _motherboards = [];
    public IReadOnlyCollection<Motherboard> Motherboards => _motherboards;

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
