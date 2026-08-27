using PcBuilderBackend.Domain.Enums;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Domain.Entities;

public class CpuCooler : ProductEntity
{
    public int MaxTdp { get; private set; }
    public CpuCoolerType Type { get; private set; }
    public decimal? CoolerHeightMm { get; private set; }
    public decimal? MaxRamHeightMm { get; private set; }
    public RadiatorLength? RadiatorLength { get; private set; }

    private readonly List<CpuCoolerSocket> _cpuCoolerSockets = [];
    public IReadOnlyCollection<CpuCoolerSocket> CpuCoolerSockets => _cpuCoolerSockets;

    protected CpuCooler()
    {
    }

    public CpuCooler(Guid manufacturerId, string name, int maxTdp, CpuCoolerType type, decimal? coolerHeightMm,
        decimal? maxRamHeightMm, RadiatorLength? radiatorLength)
    {
        SetName(name);
        SetManufacturer(manufacturerId);
        ApplyTypeSpecs(maxTdp, type, coolerHeightMm, maxRamHeightMm, radiatorLength);
    }

    public void UpdateSpecs(int maxTdp, CpuCoolerType type, decimal? coolerHeightMm,
        decimal? maxRamHeightMm, RadiatorLength? radiatorLength)
    {
        ApplyTypeSpecs(maxTdp, type, coolerHeightMm, maxRamHeightMm, radiatorLength);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void AddCpuCoolerSocket(CpuCoolerSocket cpuCoolerSocket)
    {
        if (_cpuCoolerSockets.Any(x => x.SocketId == cpuCoolerSocket.SocketId))
            throw new ArgumentException("The cpu cooler socket already exists.");

        _cpuCoolerSockets.Add(cpuCoolerSocket);
    }

    public void RemoveCpuCoolerSocket(CpuCoolerSocket cpuCoolerSocket)
    {
        if (_cpuCoolerSockets.All(x => x.SocketId != cpuCoolerSocket.SocketId))
            throw new ArgumentException("The cpu cooler socket does not exist.");

        _cpuCoolerSockets.Remove(cpuCoolerSocket);
    }

    public PartsCompatibilityResult CheckCompatibility(Cpu cpu)
    {
        if (_cpuCoolerSockets.All(x => x.SocketId != cpu.SocketId))
            return PartsCompatibilityResult.Incompatible(CompatibilityReason.MissingCpuCoolerSocket);

        return MaxTdp < cpu.ThermalDesignPower
            ? PartsCompatibilityResult.Incompatible(CompatibilityReason.ExceedsThermalDesignPower)
            : PartsCompatibilityResult.Compatible();
    }

    public PartsCompatibilityResult CheckCompatibility(Ram ram)
    {
        if (Type != CpuCoolerType.Air || !CoolerHeightMm.HasValue || !MaxRamHeightMm.HasValue)
            return PartsCompatibilityResult.Compatible();

        return ram.HeightMm > MaxRamHeightMm.Value
            ? PartsCompatibilityResult.Incompatible(CompatibilityReason.RamHeightExceedsCoolerLimit)
            : PartsCompatibilityResult.Compatible();
    }

    private void ApplyTypeSpecs(
        int maxTdp,
        CpuCoolerType type,
        decimal? coolerHeightMm,
        decimal? maxRamHeightMm,
        RadiatorLength? radiatorLength)
    {
        if (!Enum.IsDefined(type))
            throw new ArgumentException("Invalid cooler type.", nameof(type));

        Type = type;
        if (type == CpuCoolerType.Air)
        {
            if (coolerHeightMm is null)
                throw new ArgumentException("CoolerHeightMm is required for air coolers.", nameof(coolerHeightMm));
            if (maxRamHeightMm is null)
                throw new ArgumentException("MaxRamHeightMm is required for air coolers.", nameof(maxRamHeightMm));

            SetAirCoolerSpecs(maxTdp, coolerHeightMm.Value, maxRamHeightMm.Value);
        }
        else
        {
            if (radiatorLength is null)
                throw new ArgumentException("RadiatorLength is required for liquid coolers.", nameof(radiatorLength));

            SetLiquidCoolerSpecs(maxTdp, radiatorLength.Value);
        }
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
        RadiatorLength = null;
    }

    private void SetLiquidCoolerSpecs(int maxTdp, RadiatorLength radiatorLength)
    {
        if (maxTdp <= 0)
            throw new ArgumentException("MaxTdp must be greater than zero.", nameof(maxTdp));

        if (!Enum.IsDefined(radiatorLength))
            throw new ArgumentException("Invalid Radiator Length.", nameof(radiatorLength));

        MaxTdp = maxTdp;
        RadiatorLength = radiatorLength;
        CoolerHeightMm = null;
        MaxRamHeightMm = null;
    }
}
