using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.CpuCoolers;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Infrastructure.Persistence.Queries;

public class CpuCoolerReadStore(PcBuilderDbContext db, IMapper mapper) : ICpuCoolerReadStore
{
    public async Task<CpuCoolerDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await db.CpuCoolers
            .AsNoTracking()
            .Where(x => x.Id == id && x.IsActive)
            .ProjectTo<CpuCoolerDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedResult<CpuCoolerListItemDto>> ListAsync(PagedRequest request,
        CancellationToken cancellationToken)
    {
        return await db.CpuCoolers
            .AsNoTracking()
            .Include(x => x.Manufacturer)
            .ApplySorting(request.SortFields, request.SortDirection)
            .ToPagedResultAsync<CpuCooler, CpuCoolerListItemDto>(
                request.PageIndex,
                request.PageSize,
                mapper.ConfigurationProvider,
                cancellationToken);
    }

    public async Task<PagedResult<CpuCoolerListItemDto>> FilterAsync(
        PagedRequest<CpuCoolerFilter> request,
        CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        var queryable = db.CpuCoolers
            .AsNoTracking()
            .WhereIf(!string.IsNullOrWhiteSpace(filter.Name), x => x.Name.Contains(filter.Name!))
            .WhereIf(filter.ManufacturerId.HasValue, x => x.ManufacturerId == filter.ManufacturerId)
            .WhereIf(filter.Type.HasValue, x => x.Type == filter.Type)
            .WhereIf(filter.MaxTdp?.Min is not null, x => x.MaxTdp >= filter.MaxTdp!.Min)
            .WhereIf(filter.MaxTdp?.Max is not null, x => x.MaxTdp <= filter.MaxTdp!.Max)
            .WhereIf(filter.CoolerHeightMm?.Min is not null,
                x => x.CoolerHeightMm >= filter.CoolerHeightMm!.Min)
            .WhereIf(filter.CoolerHeightMm?.Max is not null,
                x => x.CoolerHeightMm <= filter.CoolerHeightMm!.Max)
            .WhereIf(filter.MaxRamHeightMm?.Min is not null,
                x => x.MaxRamHeightMm >= filter.MaxRamHeightMm!.Min)
            .WhereIf(filter.MaxRamHeightMm?.Max is not null,
                x => x.MaxRamHeightMm <= filter.MaxRamHeightMm!.Max)
            .WhereIf(filter.RadiatorLength.HasValue, x => x.RadiatorLength == filter.RadiatorLength)
            .WhereIf(filter.SocketId.HasValue,
                x => x.CpuCoolerSockets.Any(s => s.SocketId == filter.SocketId && s.IsActive));

        if (filter.MotherboardId is { } motherboardId)
        {
            var motherboard = await db.Motherboards
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == motherboardId, cancellationToken);

            if (motherboard is null)
                return PagedResult<CpuCoolerListItemDto>.Empty(request);

            queryable = queryable.Where(x =>
                x.CpuCoolerSockets.Any(s => s.SocketId == motherboard.SocketId && s.IsActive));
        }

        if (!filter.CpuId.HasValue && !filter.ChassisId.HasValue && !filter.RamId.HasValue)
        {
            return await queryable
                .ApplySorting(request.SortFields, request.SortDirection)
                .ToPagedResultAsync<CpuCooler, CpuCoolerListItemDto>(
                    request.PageIndex,
                    request.PageSize,
                    mapper.ConfigurationProvider,
                    cancellationToken);
        }

        Cpu? cpu = null;
        if (filter.CpuId is { } cpuId)
        {
            cpu = await db.Cpus
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == cpuId, cancellationToken);

            if (cpu is null)
                return PagedResult<CpuCoolerListItemDto>.Empty(request);
        }

        Chassis? chassis = null;
        if (filter.ChassisId is { } chassisId)
        {
            chassis = await db.Chassis
                .AsNoTracking()
                .Include(x => x.Radiators)
                .FirstOrDefaultAsync(x => x.Id == chassisId, cancellationToken);

            if (chassis is null)
                return PagedResult<CpuCoolerListItemDto>.Empty(request);
        }

        Ram? ram = null;
        if (filter.RamId is { } ramId)
        {
            ram = await db.Rams
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == ramId, cancellationToken);

            if (ram is null)
                return PagedResult<CpuCoolerListItemDto>.Empty(request);
        }

        // Domain CheckCompatibility is not EF-translatable; filter in memory then page.
        IEnumerable<CpuCooler> coolers = await queryable
            .Include(x => x.Manufacturer)
            .Include(x => x.CpuCoolerSockets)
            .ToListAsync(cancellationToken);

        if (cpu is not null)
        {
            coolers = coolers.Where(x =>
                x.CheckCompatibility(cpu).Status != PartsCompatibility.Incompatible);
        }

        if (chassis is not null)
            coolers = coolers.Where(chassis.CheckCpuCoolerCompatibility);

        if (ram is not null)
        {
            coolers = coolers.Where(x =>
                x.CheckCompatibility(ram).Status != PartsCompatibility.Incompatible);
        }

        var list = coolers
            .ApplySorting(request.SortFields, request.SortDirection)
            .ToList();

        return new PagedResult<CpuCoolerListItemDto>
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalCount = list.Count,
            Items = mapper.Map<List<CpuCoolerListItemDto>>(list
                .Skip(request.PageIndex * request.PageSize)
                .Take(request.PageSize))
        };
    }

    public async Task<List<CpuCoolerSocketDto>> ListCpuCoolerSockets(
        Guid cpuCoolerId,
        CancellationToken cancellationToken)
    {
        return await db.CpuCoolerSockets
            .AsNoTracking()
            .Where(x => x.CpuCoolerId == cpuCoolerId)
            .OrderBy(x => x.Socket.Name)
            .ProjectTo<CpuCoolerSocketDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}

