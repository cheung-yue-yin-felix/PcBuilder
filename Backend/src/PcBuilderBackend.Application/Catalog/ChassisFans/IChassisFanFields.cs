using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.ChassisFans;

public interface IChassisFanFields
{
    string Name { get; }
    Guid ManufacturerId { get; }
    FanDiameterMm DiameterMm { get; }
    int FansCountPerPack { get; }
}
