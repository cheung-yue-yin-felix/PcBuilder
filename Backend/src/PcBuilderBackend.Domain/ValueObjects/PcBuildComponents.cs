namespace PcBuilderBackend.Domain.ValueObjects;

public sealed class PcBuildComponents
{
    public Guid ChassisId { get; init; }
    public Guid MotherboardId { get; init; }
    public Guid CpuId { get; init; }
    public Guid? CpuCoolerId { get; init; }
    public Guid RamKitId { get; init; }
    public Guid? GraphicsCardId { get; init; }
    public Guid PsuId { get; init; }
}
