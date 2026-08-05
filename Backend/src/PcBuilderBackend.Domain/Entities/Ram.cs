using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class Ram : ProductEntity
{
    public string Color { get; set; } = string.Empty;
    public DdrGeneration DdrGeneration { get; set; }
    public RamFormFactor RamFormFactor { get; set; }
    public RamRank RamRank { get; set; }
    public int MemorySizePerStickGb { get; set; }
    public int TotalMemorySizeGb { get; set; }
    public int ModulesCount { get; set; }
    public int MaxMemorySpeedMts { get; set; }
    public decimal HeightMm { get; set; }
    
    protected Ram() {}

    public Ram(
        string name,
        Guid manufacturerId,
        string color,
        DdrGeneration ddrGeneration,
        RamFormFactor ramFormFactor,
        RamRank ramRank,
        int memorySizePerStickGb,
        int totalMemorySizeGb,
        int modulesCount,
        int maxMemorySpeedMts,
        decimal heightMm)
    {
        SetName(name);
        SetManufacturer(manufacturerId);
        SetSpecs(color, ddrGeneration, ramFormFactor, ramRank, memorySizePerStickGb, totalMemorySizeGb, modulesCount, maxMemorySpeedMts, heightMm);
    }

    public void UpdateSpecs(
        string color,
        DdrGeneration ddrGeneration,
        RamFormFactor ramFormFactor,
        RamRank ramRank,
        int memorySizePerStickGb,
        int totalMemorySizeGb,
        int modulesCount,
        int maxMemorySpeedMts,
        decimal heightMm)
    {
        SetSpecs(color, ddrGeneration, ramFormFactor, ramRank, memorySizePerStickGb, totalMemorySizeGb, modulesCount, maxMemorySpeedMts, heightMm);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void SetSpecs(
        string color,
        DdrGeneration ddrGeneration,
        RamFormFactor ramFormFactor,
        RamRank ramRank,
        int memorySizePerStickGb,
        int totalMemorySizeGb,
        int modulesCount,
        int maxMemorySpeedMts,
        decimal heightMm)
    {
        if (string.IsNullOrEmpty(color))
            throw new ArgumentException("Color cannot be null or empty");
        
        var colorTrimmed = color.Trim();
        
        if (colorTrimmed.Length > 200)
            throw new ArgumentException("Length of Color must be less than 200 characters");
        
        if (!Enum.IsDefined(ddrGeneration))
            throw new ArgumentException("DDR Generation is invalid");
        
        if (!Enum.IsDefined(ramFormFactor))
            throw new ArgumentException("Ram FormFactor is invalid");
        
        if (!Enum.IsDefined(ramRank))
            throw new ArgumentException("Ram Rank is invalid");
        
        if (memorySizePerStickGb <= 0)
            throw new ArgumentException("Memory Size per Stick GB must be greater than zero");
        
        if (totalMemorySizeGb <= 0)
            throw new ArgumentException("Total Memory Size GB must be greater than zero");
        
        if (totalMemorySizeGb < memorySizePerStickGb)
            throw new ArgumentException("Total Memory Size GB must be greater than Memory Size Per Stick GB");
        
        if (modulesCount <= 0)
            throw new ArgumentException("Modules count must be greater than zero");
        
        if (maxMemorySpeedMts <= 0)
            throw new ArgumentException("Max memory speed MT/s must be greater than zero");
        
        if (heightMm <= 0)
            throw new ArgumentException("Height Mm must be greater than zero");
        
        Color = colorTrimmed;
        DdrGeneration = ddrGeneration;
        RamFormFactor = ramFormFactor;
        RamRank = ramRank;
        MemorySizePerStickGb = memorySizePerStickGb;
        TotalMemorySizeGb = totalMemorySizeGb;
        ModulesCount = modulesCount;
        MaxMemorySpeedMts = maxMemorySpeedMts;
        HeightMm = heightMm;
    }
}
