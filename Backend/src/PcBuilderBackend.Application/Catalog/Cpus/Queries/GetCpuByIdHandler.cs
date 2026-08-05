using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Cpus.Queries;

public class GetCpuByIdHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetCpuByIdQuery, CpuDto?>
{
    public async Task<CpuDto?> Handle(GetCpuByIdQuery request, CancellationToken cancellationToken)
    {
        return await context.Cpus
            .AsNoTracking()
            .Where(c => c.Id == request.Id && c.IsActive)
            .ProjectTo<CpuDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
