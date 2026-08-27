using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class CpuRamCompat : BaseEntity
{
    public Guid CpuId { get; private set; }
    public DdrGeneration DdrGeneration { get; private set; }
    public int RamModuleCount { get; private set; }
    public RamRank RamRank { get; private set; }
    public int MaxSpeedMts { get; private set; }
    public Cpu Cpu { get; private set; } = null!;

    protected CpuRamCompat()
    {
    }

    public CpuRamCompat(Guid cpuId, DdrGeneration ddrGeneration, int ramModuleCount, RamRank ramRank, int maxSpeedMts)
    {
        SetSpecs(cpuId, ddrGeneration, ramModuleCount, ramRank, maxSpeedMts);
    }

    public void UpdateSpecs(Guid cpuId, DdrGeneration ddrGeneration, int ramModuleCount, RamRank ramRank,
        int maxSpeedMts)
    {
        SetSpecs(cpuId, ddrGeneration, ramModuleCount, ramRank, maxSpeedMts);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void SetSpecs(Guid cpuId, DdrGeneration ddrGeneration, int ramModuleCount, RamRank ramRank, int maxSpeedMts)
    {
        if (cpuId == Guid.Empty)
            throw new ArgumentException("CPU ID cannot be empty");

        if (!Enum.IsDefined(ddrGeneration))
            throw new ArgumentException("DDR Generation is not valid", nameof(ddrGeneration));

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(ramModuleCount);

        if (!Enum.IsDefined(ramRank))
            throw new ArgumentException("Ram Rank is invalid");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxSpeedMts);

        CpuId = cpuId;
        DdrGeneration = ddrGeneration;
        RamModuleCount = ramModuleCount;
        RamRank = ramRank;
        MaxSpeedMts = maxSpeedMts;
    }
}
