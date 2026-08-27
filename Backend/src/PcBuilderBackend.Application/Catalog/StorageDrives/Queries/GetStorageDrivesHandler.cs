using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.StorageDrives.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Queries;

public class GetStorageDrivesHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetStorageDrivesQuery, PagedResult<StorageDriveDto>>
{
    public async Task<PagedResult<StorageDriveDto>> Handle(
        GetStorageDrivesQuery query,
        CancellationToken cancellationToken)
    {
        return await context.StorageDrives
            .AsNoTracking()
            .Include(x => x.Manufacturer)
            .ApplySorting(query.Request.SortFields, query.Request.SortDirection)
            .ToPagedResultAsync<StorageDrive, StorageDriveDto>(
                query.Request.PageIndex,
                query.Request.PageSize,
                mapper.ConfigurationProvider,
                cancellationToken);
    }
}
