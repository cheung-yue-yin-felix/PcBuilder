using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Chassis;

public interface IChassisRepository
{
    Task<Domain.Entities.Chassis?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Domain.Entities.Chassis?> GetWithChildrenAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Domain.Entities.Chassis>> GetByIdsAsync(
        IReadOnlyCollection<Guid> ids,
        CancellationToken cancellationToken);

    void Add(Domain.Entities.Chassis chassis);
    void DeleteDriveBay(ChassisDriveBay driveBay);
    void DeleteFanMount(ChassisFanMount fanMount);
    void DeleteFanMountOption(ChassisFanMountOption fanMountOption);
    void DeleteMbFormFactor(ChassisMbFormFactor mbFormFactor);
    void DeletePcieSlot(ChassisPcieSlot pcieSlot);
    void DeleteRadiator(ChassisRadiator radiator);
    void DeletePsuFormFactor(ChassisPsuFormFactor psuFormFactor);
}