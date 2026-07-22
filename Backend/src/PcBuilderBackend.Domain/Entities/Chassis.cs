using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class Chassis : ProductEntity
{
    public List<MbFormFactor> SupportedMbFormFactors { get; set; } = new List<MbFormFactor>();
    public List<PsuFormFactor> SupportedPsuFormFactors { get; set; } = new List<PsuFormFactor>();
    public double LengthMm { get; set; }
    public double WidthMm { get; set; }
    public double HeightMm { get; set; }
    public double MaxCpuCoolerHeightMm { get; set; }
    public double MaxGraphicsCardLengthMm { get; set; }
    public double MaxPsuLengthMm { get; set; }
    public virtual ICollection<ChassisFanMount> FanMounts { get; set; } = new List<ChassisFanMount>();
    public virtual ICollection<ChassisDriveBay> DriveBays { get; set; } = new List<ChassisDriveBay>();
    public virtual ICollection<ChassisPcieSlot> PcieSlots { get; set; } = new List<ChassisPcieSlot>();
    public virtual ICollection<ChassisRadiator> Radiators { get; set; } = new List<ChassisRadiator>();
}