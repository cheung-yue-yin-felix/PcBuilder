using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class ChassisFan : ProductEntity
{
    public FanDiameterMm DiameterMm { get; private set; }
    public int FansCountPerPack { get; private set; }
    
    protected ChassisFan() {}

    public ChassisFan(string name, Guid manufacturerId, FanDiameterMm diameterMm, int fansCountPerPack)
    {
        SetName(name);
        SetManufacturer(manufacturerId);
        SetSpecs(diameterMm, fansCountPerPack);
    }

    public void UpdateSpecs(FanDiameterMm diameterMm, int fansCountPerPack)
    {
        SetSpecs(diameterMm, fansCountPerPack);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void SetSpecs(FanDiameterMm diameterMm, int fansCountPerPack)
    {
        if (!Enum.IsDefined(typeof(FanDiameterMm), diameterMm))
            throw new ArgumentException("Fan Diameter mm is invalid");
        
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(fansCountPerPack);
        
        DiameterMm = diameterMm;
        FansCountPerPack = fansCountPerPack;
    }
}
