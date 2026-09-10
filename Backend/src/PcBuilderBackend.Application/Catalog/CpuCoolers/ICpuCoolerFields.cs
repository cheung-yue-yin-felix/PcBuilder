using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.CpuCoolers;

public interface ICpuCoolerFields
{
    string Name { get; }
    Guid ManufacturerId { get; }
    int MaxTdp { get; }
    CpuCoolerType Type { get; }
    decimal? CoolerHeightMm { get; }
    decimal? MaxRamHeightMm { get; }
    RadiatorLength? RadiatorLength { get; }
}
