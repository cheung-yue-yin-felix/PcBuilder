using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class Psu : ProductEntity
{
    public int Wattage { get; set; }
    public PsuModularity Modularity { get; set; }
    public PsuFormFactor FormFactor { get; set; }
    public decimal LengthMm { get; set; }
    public decimal WidthMm { get; set; }
    public decimal HeightMm { get; set; }
    public ICollection<PsuCable> Cables { get; set; } = new List<PsuCable>();
    
    protected Psu() {}

    public Psu(string name, Guid manufacturerId, int wattage, PsuModularity modularity, decimal lengthMm,
        decimal widthMm, decimal heightMm)
    {
        SetName(name);
        SetManufacturer(manufacturerId);
        SetSpecs(wattage, modularity, lengthMm, widthMm, heightMm);
    }

    public void UpdateSpecs(int wattage, PsuModularity modularity, decimal lengthMm, decimal widthMm, decimal heightMm)
    {
        SetSpecs(wattage, modularity, lengthMm, widthMm, heightMm);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void AddCable(PsuCable cable)
    {
        if (Cables.Any(c => c.PsuId == cable.PsuId && c.Type == cable.Type))
            throw new ArgumentException("Cable is already added");
            
        Cables.Add(cable);
    }

    public void RemoveCable(PsuCable cable)
    {
        if (!Cables.Any(c => c.PsuId == cable.PsuId && c.Type == cable.Type))
            throw new ArgumentException("Cable does not exist");
        
        Cables.Remove(cable);
    }
    
    private void SetSpecs(int wattage, PsuModularity modularity, decimal lengthMm,
        decimal widthMm, decimal heightMm)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(wattage);
        
        if (!Enum.IsDefined(modularity))
            throw new ArgumentException("Modularity is invalid  ");
        
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(lengthMm);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(widthMm);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(heightMm);
        
        Wattage = wattage;
        Modularity = modularity;
        LengthMm = lengthMm;
        WidthMm = widthMm;
        HeightMm = heightMm;
    }
}
