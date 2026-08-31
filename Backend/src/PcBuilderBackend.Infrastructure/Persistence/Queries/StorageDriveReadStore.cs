using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.StorageDrives;
using PcBuilderBackend.Application.Catalog.StorageDrives.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Infrastructure.Persistence.Queries;

public sealed class StorageDriveReadStore(PcBuilderDbContext db, IMapper mapper) : IStorageDriveReadStore
{
    public Task<StorageDriveDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        db.StorageDrives
            .AsNoTracking()
            .Where(x => x.Id == id)
            .ProjectTo<StorageDriveDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<PagedResult<StorageDriveDto>> ListAsync(PagedRequest request, CancellationToken cancellationToken) =>
        db.StorageDrives
            .AsNoTracking()
            .Include(x => x.Manufacturer)
            .ApplySorting(request.SortFields, request.SortDirection)
            .ToPagedResultAsync<StorageDrive, StorageDriveDto>(
                request.PageIndex,
                request.PageSize,
                mapper.ConfigurationProvider,
                cancellationToken);

    public async Task<PagedResult<StorageDriveDto>> FilterAsync(
        PagedRequest<StorageDriveFilter> request,
        CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        var queryable = db.StorageDrives.AsNoTracking()
            .Include(x => x.Manufacturer)
            .WhereIf(!string.IsNullOrWhiteSpace(filter.Name), x => x.Name.Contains(filter.Name!))
            .WhereIf(filter.ManufacturerId.HasValue, x => x.ManufacturerId == filter.ManufacturerId)
            .WhereIf(filter.Media.HasValue, x => x.Media == filter.Media)
            .WhereIf(filter.Interface.HasValue, x => x.Interface == filter.Interface)
            .WhereIf(filter.FormFactor.HasValue, x => x.FormFactor == filter.FormFactor)
            .WhereIf(filter.CapacityGb is not null,
                x => x.CapacityGb >= filter.CapacityGb!.Min && x.CapacityGb <= filter.CapacityGb!.Max)
            .WhereIf(filter.PcieGeneration.HasValue, x => x.PcieGeneration == filter.PcieGeneration)
            .WhereIf(filter.Rpm is not null,
                x => x.Rpm >= filter.Rpm!.Min && x.Rpm <= filter.Rpm!.Max);

        if (!filter.MotherboardId.HasValue && !filter.ChassisId.HasValue)
        {
            return await queryable
                .ApplySorting(request.SortFields, request.SortDirection)
                .ToPagedResultAsync<StorageDrive, StorageDriveDto>(
                    request.PageIndex,
                    request.PageSize,
                    mapper.ConfigurationProvider,
                    cancellationToken);
        }

        Motherboard? motherboard = null;
        if (filter.MotherboardId is { } motherboardId)
        {
            motherboard = await db.Motherboards
                .AsNoTracking()
                .Include(m => m.M2Slots)
                .ThenInclude(s => s.FormFactors)
                .FirstOrDefaultAsync(m => m.Id == motherboardId, cancellationToken);

            if (motherboard is null)
                return PagedResult<StorageDriveDto>.Empty(request);
        }

        Chassis? chassis = null;
        if (filter.ChassisId is { } chassisId)
        {
            chassis = await db.Chassis
                .AsNoTracking()
                .Include(c => c.DriveBays)
                .FirstOrDefaultAsync(c => c.Id == chassisId, cancellationToken);

            if (chassis is null)
                return PagedResult<StorageDriveDto>.Empty(request);
        }

        IEnumerable<StorageDrive> drives = await queryable.ToListAsync(cancellationToken);

        if (motherboard is not null)
        {
            drives = drives.Where(x =>
                motherboard.CheckStorageCompatibility(x).Status != PartsCompatibility.Incompatible);
        }

        if (chassis is not null)
            drives = drives.Where(chassis.CheckStorageDriveCompatibility);

        var list = drives
            .ApplySorting(request.SortFields, request.SortDirection)
            .ToList();

        return new PagedResult<StorageDriveDto>
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalCount = list.Count,
            Items = mapper.Map<List<StorageDriveDto>>(list
                .Skip(request.PageIndex * request.PageSize)
                .Take(request.PageSize))
        };
    }
}
