using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Chassis;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Infrastructure.Persistence.Queries;

public class ChassisReadStore(IApplicationDbContext db, IMapper mapper) : IChassisReadStore
{
    public async Task <ChassisDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await db.Chassis
            .AsNoTracking()
            .Include(x => x.FanMounts)
            .ThenInclude(x => x.Options)
            .Include(x => x.DriveBays)
            .Include(x => x.PcieSlots)
            .Include(x => x.Radiators)
            .Include(x => x.PsuFormFactors)
            .Include(x => x.MbFormFactors)
            .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, cancellationToken);

        return entity is null ? null : mapper.Map<ChassisDto>(entity);
    }

    public async Task<PagedResult<ChassisListItemDto>> ListAsync(
        PagedRequest request,
        CancellationToken cancellationToken)
    {
        return await db.Chassis
            .AsNoTracking()
            .ApplySorting(request.SortFields, request.SortDirection)
            .ToPagedResultAsync<Domain.Entities.Chassis, ChassisListItemDto>(
                request.PageIndex,
                request.PageSize,
                mapper.ConfigurationProvider,
                cancellationToken);
    }

    public async Task<PagedResult<ChassisListItemDto>> FilterAsync(
        PagedRequest<ChassisFilter> request,
        CancellationToken cancellationToken)
    {
        return await db.Chassis
            .AsNoTracking()
            .Include(x => x.MbFormFactors)
            .WhereIf(!string.IsNullOrWhiteSpace(request.Filter.Name),
                x => x.Name.Contains(request.Filter.Name!))
            .WhereIf(request.Filter.ManufacturerId.HasValue,
                x => x.ManufacturerId == request.Filter.ManufacturerId)
            .WhereIf(request.Filter.SupportedMbFormFactors.Count != 0,
                x => x.MbFormFactors.Any(f => request.Filter.SupportedMbFormFactors.Contains(f.MbFormFactor)))
            .WhereIf(request.Filter.HeightMm != null,
                x => x.HeightMm <= request.Filter.HeightMm!.Max &&
                     x.HeightMm >= request.Filter.HeightMm!.Min)
            .WhereIf(request.Filter.LengthMm != null,
                x => x.LengthMm <= request.Filter.LengthMm!.Max &&
                     x.LengthMm >= request.Filter.LengthMm!.Min)
            .WhereIf(request.Filter.WidthMm != null,
                x => x.WidthMm <= request.Filter.WidthMm!.Max && x.WidthMm >= request.Filter.WidthMm!.Min)
            .WhereIf(request.Filter.MotherboardMaxWidthMm != null,
                x => x.MotherboardMaxWidthMm <= request.Filter.MotherboardMaxWidthMm!.Max &&
                     x.MotherboardMaxWidthMm >= request.Filter.MotherboardMaxWidthMm!.Min)
            .WhereIf(request.Filter.MotherboardMaxHeightMm != null,
                x => x.MotherboardMaxHeightMm <= request.Filter.MotherboardMaxHeightMm!.Max &&
                     x.MotherboardMaxHeightMm >= request.Filter.MotherboardMaxHeightMm!.Min)
            .WhereIf(request.Filter.MaxCpuCoolerHeightMm != null,
                x => x.MaxCpuCoolerHeightMm <= request.Filter.MaxCpuCoolerHeightMm!.Max &&
                     x.MaxCpuCoolerHeightMm >= request.Filter.MaxCpuCoolerHeightMm!.Min)
            .WhereIf(request.Filter.MaxGraphicsCardLengthMm != null,
                x => x.MaxGraphicsCardLengthMm <= request.Filter.MaxGraphicsCardLengthMm!.Max &&
                     x.MaxGraphicsCardLengthMm >= request.Filter.MaxGraphicsCardLengthMm!.Min)
            .WhereIf(request.Filter.MaxPsuLengthMm != null,
                x => x.MaxPsuLengthMm <= request.Filter.MaxPsuLengthMm!.Max &&
                     x.MaxPsuLengthMm >= request.Filter.MaxPsuLengthMm!.Min)
            .ApplySorting(request.SortFields, request.SortDirection)
            .ToPagedResultAsync<Domain.Entities.Chassis, ChassisListItemDto>(
                request.PageIndex,
                request.PageSize,
                mapper.ConfigurationProvider,
                cancellationToken);
    }

    public async Task<List<ChassisDriveBayDto>> ListDriveBaysAsync(Guid chassisId, CancellationToken cancellationToken)
    {
        var chassis = await db.Chassis
            .AsNoTracking()
            .Include(x => x.DriveBays)
            .FirstOrDefaultAsync(x => x.Id == chassisId && x.IsActive, cancellationToken);

        if (chassis is null)
            return [];

        return mapper.Map<List<ChassisDriveBayDto>>(
            chassis.DriveBays.Where(x => x.IsActive).OrderBy(x => x.DriveBayFormFactor).ToList());
    }

    public async Task<List<ChassisFanMountDto>> ListFanMountsAsync(Guid chassisId, CancellationToken cancellationToken)
    {
        var chassis = await db.Chassis
            .AsNoTracking()
            .Include(x => x.FanMounts)
            .ThenInclude(x => x.Options)
            .FirstOrDefaultAsync(x => x.Id == chassisId && x.IsActive, cancellationToken);

        if (chassis is null)
            return [];

        return mapper.Map<List<ChassisFanMountDto>>(
            chassis.FanMounts.Where(x => x.IsActive).OrderBy(x => x.Location).ToList());
    }

    public async Task<List<MbFormFactor>> ListMbFormFactorsAsync(
        Guid chassisId,
        CancellationToken cancellationToken)
    {
        var chassis = await db.Chassis
            .AsNoTracking()
            .Include(x => x.MbFormFactors)
            .FirstOrDefaultAsync(x => x.Id == chassisId && x.IsActive, cancellationToken);

        if (chassis is null)
            return [];

        return
        [
            .. chassis.MbFormFactors
                .Where(x => x.IsActive)
                .Select(x => x.MbFormFactor)
                .OrderBy(x => x)
        ];
    }

    public async Task<List<ChassisPcieSlotDto>> ListPcieSlotsAsync(Guid chassisId, CancellationToken cancellationToken)
    {
        var chassis = await db.Chassis
            .AsNoTracking()
            .Include(x => x.PcieSlots)
            .FirstOrDefaultAsync(x => x.Id == chassisId && x.IsActive, cancellationToken);

        if (chassis is null)
            return [];

        return mapper.Map<List<ChassisPcieSlotDto>>(
            chassis.PcieSlots
                .Where(x => x.IsActive)
                .OrderBy(x => x.Orientation)
                .ThenBy(x => x.LowProfileSlots)
                .ToList());
    }

    public async Task<List<PsuFormFactor>> ListPsuFormFactorsAsync(Guid chassisId, CancellationToken cancellationToken)
    {
        var chassis = await db.Chassis
            .AsNoTracking()
            .Include(x => x.PsuFormFactors)
            .FirstOrDefaultAsync(x => x.Id == chassisId && x.IsActive, cancellationToken);

        if (chassis is null)
            return [];

        return
        [
            .. chassis.PsuFormFactors
                .Where(x => x.IsActive)
                .Select(x => x.PsuFormFactor)
                .OrderBy(x => x)
        ];
    }

    public async Task<List<ChassisRadiatorDto>> ListRadiatorsAsync(Guid chassisId, CancellationToken cancellationToken)
    {
        var chassis = await db.Chassis
            .AsNoTracking()
            .Include(x => x.Radiators)
            .FirstOrDefaultAsync(x => x.Id == chassisId && x.IsActive, cancellationToken);

        if (chassis is null)
            return [];

        return mapper.Map<List<ChassisRadiatorDto>>(
            chassis.Radiators
                .Where(x => x.IsActive)
                .OrderBy(x => x.MountLocation)
                .ThenBy(x => x.Length)
                .ToList());
    }
}