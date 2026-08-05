using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class CpuCooler : ProductEntity
{
    public int MaxTdp { get; set; }
    public CpuCoolerType Type { get; set; }
    public decimal? CoolerHeightMm { get; set; }
    public decimal? MaxRamHeightMm { get; set; }
    public RadiatorLength? RadiatorLength { get; set; }
    public ICollection<CpuCoolerSocket> CpuCoolerSockets { get; } = new List<CpuCoolerSocket>();
    
    protected CpuCooler() {}

    public CpuCooler(Guid manufacturerId, string name, int maxTdp, CpuCoolerType type, decimal? coolerHeightMm,
        decimal? maxRamHeightMm, RadiatorLength? radiatorLength)
    {
        SetName(name);
        SetManufacturer(manufacturerId);
        Type = type;
        if (type == CpuCoolerType.Air)
            SetAirCoolerSpecs(maxTdp, coolerHeightMm!.Value, maxRamHeightMm!.Value);
        else
            SetLiquidCoolerSpecs(maxTdp, radiatorLength!.Value);
    }

    public void UpdateSpecs(int maxTdp, CpuCoolerType type, decimal? coolerHeightMm,
        decimal? maxRamHeightMm, RadiatorLength? radiatorLength)
    {
        Type = type;
        if (type == CpuCoolerType.Air)
            SetAirCoolerSpecs(maxTdp, coolerHeightMm!.Value, maxRamHeightMm!.Value);
        else
            SetLiquidCoolerSpecs(maxTdp, radiatorLength!.Value);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void AddCpuCoolerSocket(CpuCoolerSocket cpuCoolerSocket)
    {
        if (CpuCoolerSockets.Any(x => x.SocketId == cpuCoolerSocket.SocketId))
            throw new ArgumentException("The cpu cooler socket already exists.");
        
        CpuCoolerSockets.Add(cpuCoolerSocket);
    }
    
    public void RemoveCpuCoolerSocket(CpuCoolerSocket cpuCoolerSocket)
    {
        if (!CpuCoolerSockets.Any(x => x.SocketId == cpuCoolerSocket.SocketId))
            throw new ArgumentException("The cpu cooler socket does not exist.");
        
        CpuCoolerSockets.Remove(cpuCoolerSocket);
    }

    private void SetAirCoolerSpecs(int maxTdp, decimal coolerHeightMm, decimal maxRamHeightMm)
    {
        if (maxTdp <= 0)
            throw new ArgumentException("MaxTdp must be greater than zero.", nameof(maxTdp));
        
        if (coolerHeightMm <= 0)
            throw new ArgumentException("CoolerHeightMm must be greater than zero.", nameof(coolerHeightMm));
        
        if (maxRamHeightMm <= 0)
            throw new ArgumentException("MaxRamHeightMm must be greater than zero.", nameof(maxRamHeightMm));
        
        MaxTdp = maxTdp;
        CoolerHeightMm = coolerHeightMm;
        MaxRamHeightMm = maxRamHeightMm;
    }

    private void SetLiquidCoolerSpecs(int maxTdp, RadiatorLength radiatorLength)
    {
        if (maxTdp <= 0)
            throw new ArgumentException("MaxTdp must be greater than zero.", nameof(maxTdp));
        
        if (!Enum.IsDefined(radiatorLength))
            throw new ArgumentException("Invalid Radiator Length.", nameof(radiatorLength));
        
        MaxTdp = maxTdp;
        RadiatorLength = radiatorLength;
    }
}
