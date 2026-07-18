using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class CpuCooler : BaseEntity
{
    public Guid ManufacturerId { get; set; }

    public string Name { get; set; } = string.Empty;
    
    public int MaxTdp { get; set; }
    
    public CpuCoolerType Type { get; set; }
    
    public double? CoolerHeightMm { get; set; }
    
    public double? MaxRamHeightMm { get; set; }
    
    public RadiatorLength? RadiatorLength { get; set; }

    public virtual Manufacturer Manufacturer { get; set; } = null!;
}