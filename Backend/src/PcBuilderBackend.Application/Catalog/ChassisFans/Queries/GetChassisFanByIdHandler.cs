using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.ChassisFans.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.ChassisFans.Queries;

public class GetChassisFanByIdHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetChassisFanByIdQuery, ChassisFanDto?>
{
    public async Task<ChassisFanDto?> Handle(GetChassisFanByIdQuery request, CancellationToken cancellationToken)
    {
        return await context.ChassisFans
            .AsNoTracking()
            .Include(x => x.Manufacturer)
            .Where(x => x.Id == request.Id && x.IsActive)
            .ProjectTo<ChassisFanDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}