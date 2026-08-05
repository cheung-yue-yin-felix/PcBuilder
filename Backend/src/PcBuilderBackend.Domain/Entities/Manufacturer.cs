namespace PcBuilderBackend.Domain.Entities;

public class Manufacturer : BaseEntity
{
    protected Manufacturer()
    {
    }

    public Manufacturer(string name)
    {
        SetName(name);
    }

    public string Name { get; private set; } = string.Empty;

    public ICollection<Socket> Sockets { get; set; } = new List<Socket>();
    public ICollection<Chipset> Chipsets { get; set; } = new List<Chipset>();
    public ICollection<Gpu> Gpus { get; set; } = new List<Gpu>();
    public ICollection<Cpu> Cpus { get; set; } = new List<Cpu>();
    public ICollection<Ram> Rams { get; set; } = new List<Ram>();
    public ICollection<GraphicsCard> GraphicsCards { get; set; } = new List<GraphicsCard>();
    public ICollection<Motherboard> Motherboards { get; set; } = new List<Motherboard>();
    public ICollection<Psu> Psus { get; set; } = new List<Psu>();
    public ICollection<Chassis> Chassis { get; set; } = new List<Chassis>();
    public ICollection<StorageDrive> StorageDrives { get; set; } = new List<StorageDrive>();
    public ICollection<CpuCooler> CpuCoolers { get; set; } = new List<CpuCooler>();
    public ICollection<CpuSeries> CpuSeries { get; set; } = new List<CpuSeries>();
    public ICollection<GpuSeries> GpuSeries { get; set; } = new List<GpuSeries>();

    public ICollection<WiredNetworkAdapter> WiredNetworkAdapters { get; set; } =
        new List<WiredNetworkAdapter>();

    public ICollection<WirelessNetworkAdapter> WirelessNetworkAdapters { get; set; } =
        new List<WirelessNetworkAdapter>();

    public ICollection<ChassisFan> ChassisFans { get; set; } = new List<ChassisFan>();

    public void Rename(string name)
    {
        SetName(name);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void SetName(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Manufacturer name is required.", nameof(name));

        var trimmed = name.Trim();
        if (trimmed.Length > 200)
            throw new ArgumentException("Manufacturer name must be 200 characters or fewer.", nameof(name));

        Name = trimmed;
    }
}
