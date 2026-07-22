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

    public virtual ICollection<Socket> Sockets { get; set; } = new List<Socket>();
    public virtual ICollection<Chipset> Chipsets { get; set; } = new List<Chipset>();
    public virtual ICollection<Gpu> Gpus { get; set; } = new List<Gpu>();
    public virtual ICollection<Cpu> Cpus { get; set; } = new List<Cpu>();
    public virtual ICollection<Ram> Rams { get; set; } = new List<Ram>();
    public virtual ICollection<GraphicsCard> GraphicsCards { get; set; } = new List<GraphicsCard>();
    public virtual ICollection<Motherboard> Motherboards { get; set; } = new List<Motherboard>();
    public virtual ICollection<Psu> Psus { get; set; } = new List<Psu>();
    public virtual ICollection<Chassis> Chassis { get; set; } = new List<Chassis>();
    public virtual ICollection<StorageDrive> StorageDrives { get; set; } = new List<StorageDrive>();
    public virtual ICollection<CpuCooler> CpuCoolers { get; set; } = new List<CpuCooler>();

    public virtual ICollection<WiredNetworkAdapter> WiredNetworkAdapters { get; set; } =
        new List<WiredNetworkAdapter>();

    public virtual ICollection<WirelessNetworkAdapter> WirelessNetworkAdapters { get; set; } =
        new List<WirelessNetworkAdapter>();

    public virtual ICollection<ChassisFan> ChassisFans { get; set; } = new List<ChassisFan>();

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