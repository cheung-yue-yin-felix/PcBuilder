using PcBuilderBackend.Application.Catalog.Chassis.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis;

public interface IChassisReadStore
{
    Task<ChassisDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PagedResult<ChassisListItemDto>> ListAsync(
        PagedRequest request,
        CancellationToken cancellationToken);
    Task<PagedResult<ChassisListItemDto>> FilterAsync(
        PagedRequest<ChassisFilter> request,
        CancellationToken cancellationToken);
    Task<List<ChassisDriveBayDto>> ListDriveBaysAsync(
        Guid chassisId,
        CancellationToken cancellationToken);
    Task<List<ChassisFanMountDto>> ListFanMountsAsync(
        Guid chassisId,
        CancellationToken cancellationToken);
    Task<List<MbFormFactor>> ListMbFormFactorsAsync(
        Guid chassisId,
        CancellationToken cancellationToken);
    Task<List<ChassisPcieSlotDto>> ListPcieSlotsAsync(
        Guid chassisId,
        CancellationToken cancellationToken);
    Task<List<PsuFormFactor>> ListPsuFormFactorsAsync(
        Guid chassisId,
        CancellationToken cancellationToken);
    Task<List<ChassisRadiatorDto>> ListRadiatorsAsync(
        Guid chassisId,
        CancellationToken cancellationToken);
}