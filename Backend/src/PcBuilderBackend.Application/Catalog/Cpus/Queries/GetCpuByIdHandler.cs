using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Cpus.Queries;

public class GetCpuByIdHandler(IApplicationDbContext context, IMapper mapper): IRequestHandler<GetCpuByIdQuery, CpuDto?>
{
    public async Task<CpuDto?> Handle(GetCpuByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await context.Cpus
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.Id && c.IsActive, cancellationToken);
        return entity == null ? null : mapper.Map<CpuDto>(entity);
    }
}