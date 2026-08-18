namespace PcBuilderBackend.Domain.Entities;

public class CpuSupportChipset : BaseEntity
{
    public Guid CpuId { get; set; }
    public Guid ChipsetId { get; set; }
    public bool RequiresBiosUpdate { get; set; }
    public Cpu Cpu { get; init; } = null!;
    public Chipset Chipset { get; init; } = null!;

    protected CpuSupportChipset() {}

    public CpuSupportChipset(Guid cpuId, Guid chipsetId, bool requiresBiosUpdate = false)
    {
        SetSpecs(cpuId, chipsetId, requiresBiosUpdate);
    }

    public void UpdateSpecs(Guid cpuId, Guid chipsetId, bool requiresBiosUpdate)
    {
        SetSpecs(cpuId, chipsetId, requiresBiosUpdate);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void SetSpecs(Guid cpuId, Guid chipsetId, bool requiresBiosUpdate)
    {
        if (cpuId == Guid.Empty)
            throw new ArgumentException("CPU ID cannot be empty", nameof(cpuId));

        if (chipsetId == Guid.Empty)
            throw new ArgumentException("Chipset ID cannot be empty", nameof(chipsetId));

        CpuId = cpuId;
        ChipsetId = chipsetId;
        RequiresBiosUpdate = requiresBiosUpdate;
    }
}
