using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.StorageDrives.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Queries;

public class GetStorageDriveByIdHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetStorageDriveByIdQuery, StorageDriveDto?>
{
    public async Task<StorageDriveDto?> Handle(
        GetStorageDriveByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await context.StorageDrives
            .AsNoTracking()
            .Include(x => x.Manufacturer)
            .Where(x => x.Id == request.Id && x.IsActive)
            .ProjectTo<StorageDriveDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
