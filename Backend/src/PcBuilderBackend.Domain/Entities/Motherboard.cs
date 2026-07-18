using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class Motherboard : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public Guid ManufacturerId { get; set; }

    public Guid SocketId { get; set; }

    public Guid ChipsetId { get; set; }

    public int RamSlots { get; set; }

    public int MaxMemoryGb { get; set; }

    public int MaxDimmSizeGb { get; set; }
    
    public int SataPorts { get; set; }
    
    public int FanConnectors { get; set; }
    
    public int EpsConnectors { get; set; }
    
    public decimal WidthMm { get; set; }
    
    public decimal HeightMm { get; set; }

    public DdrGeneration DdrGeneration { get; set; }

    public RamFormFactor RamFormFactor { get; set; }
    
    public MbFormFactor FormFactor { get; set; }
    
    public bool WifiEnabled { get; set; }
    
    public bool BluetoothEnabled { get; set; }

    public List<int> SupportedMemorySpeeds { get; set; } = new List<int>();

    public virtual Chipset Chipset { get; set; } = null!;

    public virtual Socket Socket { get; set; } = null!;

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual ICollection<MotherboardPcie> PcieSlots { get; set; } = new List<MotherboardPcie>();

    public virtual ICollection<MotherboardM2> M2Slots { get; set; } = new List<MotherboardM2>();
}