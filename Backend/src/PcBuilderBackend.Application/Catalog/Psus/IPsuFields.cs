using PcBuilderBackend.Application.Catalog.Psus.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Psus;

public interface IPsuFields
{
    string Name { get; }
    Guid ManufacturerId { get; }
    int Wattage { get; }
    PsuModularity Modularity { get; }
    PsuFormFactor FormFactor { get; }
    decimal LengthMm { get; }
    decimal WidthMm { get; }
    decimal HeightMm { get; }
}

public interface IPsuCables
{
    List<PsuCableDto> Cables { get; }
}
