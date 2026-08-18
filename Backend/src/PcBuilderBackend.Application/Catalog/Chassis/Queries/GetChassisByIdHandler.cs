using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Chassis.Queries;

public class GetChassisByIdHandler(IApplicationDbContext dbContext, IMapper mapper) : IRequestHandler<GetChassisByIdQuery, ChassisDto?>
{
    public async Task<ChassisDto?> Handle(GetChassisByIdQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Chassis
            .AsNoTracking()
            .Include(x => x.MbFormFactors)
            .Where(x => x.Id == request.Id)
            .ProjectTo<ChassisDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}