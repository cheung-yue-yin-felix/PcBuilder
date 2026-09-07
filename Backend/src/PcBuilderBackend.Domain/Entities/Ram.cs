using PcBuilderBackend.Domain.Enums;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Domain.Entities;

public class Ram : ProductEntity
{
    public string Color { get; private set; } = string.Empty;
    public DdrGeneration DdrGeneration { get; private set; }
    public RamFormFactor RamFormFactor { get; private set; }
    public RamRank RamRank { get; private set; }
    public int MemorySizePerStickGb { get; private set; }
    public int TotalMemorySizeGb { get; private set; }
    public int ModulesCount { get; private set; }
    public int MaxMemorySpeedMts { get; private set; }
    public decimal HeightMm { get; private set; }
    
    protected Ram() {}

    public Ram(string name, Guid manufacturerId, RamSpecs specs)
    {
        SetName(name);
        SetManufacturer(manufacturerId);
        SetSpecs(specs);
    }

    public void UpdateSpecs(RamSpecs specs)
    {
        SetSpecs(specs);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void SetSpecs(RamSpecs specs)
    {
        if (string.IsNullOrEmpty(specs.Color))
            throw new ArgumentException("Color cannot be null or empty");

        var colorTrimmed = specs.Color.Trim();

        if (colorTrimmed.Length > 200)
            throw new ArgumentException("Length of Color must be less than 200 characters");

        if (!Enum.IsDefined(specs.DdrGeneration))
            throw new ArgumentException("DDR Generation is invalid");

        if (!Enum.IsDefined(specs.RamFormFactor))
            throw new ArgumentException("Ram FormFactor is invalid");

        if (!Enum.IsDefined(specs.RamRank))
            throw new ArgumentException("Ram Rank is invalid");

        if (specs.MemorySizePerStickGb <= 0)
            throw new ArgumentException("Memory Size per Stick GB must be greater than zero");

        if (specs.TotalMemorySizeGb <= 0)
            throw new ArgumentException("Total Memory Size GB must be greater than zero");

        if (specs.TotalMemorySizeGb < specs.MemorySizePerStickGb)
            throw new ArgumentException("Total Memory Size GB must be greater than Memory Size Per Stick GB");

        if (specs.ModulesCount <= 0)
            throw new ArgumentException("Modules count must be greater than zero");

        if (specs.MaxMemorySpeedMts <= 0)
            throw new ArgumentException("Max memory speed MT/s must be greater than zero");

        if (specs.HeightMm <= 0)
            throw new ArgumentException("Height Mm must be greater than zero");

        Color = colorTrimmed;
        DdrGeneration = specs.DdrGeneration;
        RamFormFactor = specs.RamFormFactor;
        RamRank = specs.RamRank;
        MemorySizePerStickGb = specs.MemorySizePerStickGb;
        TotalMemorySizeGb = specs.TotalMemorySizeGb;
        ModulesCount = specs.ModulesCount;
        MaxMemorySpeedMts = specs.MaxMemorySpeedMts;
        HeightMm = specs.HeightMm;
    }
}
