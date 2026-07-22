using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class ChassisFan : NamedEntity
{
    public Guid ManufacturerId { get; set; }
    public FanDiameterMm DiameterMm { get; set; }
    public int FansCountPerPack { get; set; }
    public virtual Manufacturer Manufacturer { get; set; } = null!;
}