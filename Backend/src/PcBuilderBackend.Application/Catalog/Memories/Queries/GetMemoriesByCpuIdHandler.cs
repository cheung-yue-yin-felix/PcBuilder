using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Memories.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Memories.Queries;

public class GetMemoriesByCpuIdHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetMemoriesByCpuIdQuery, List<RamDto>>
{
    public async Task<List<RamDto>> Handle(
        GetMemoriesByCpuIdQuery request,
        CancellationToken cancellationToken)
    {
        var cpu = await context.Cpus.AsNoTracking()
            .Where(cpu => cpu.Id == request.CpuId && cpu.IsActive)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (cpu is null)
            return new List<RamDto>();

        return await context.Rams.AsNoTracking()
            .Where(ram => ram.IsActive)
            .Where(ram => cpu.CheckMemoryCompatibility(ram).Status != PartsCompatibility.Incompatible)
            .OrderBy(x => x.Name)
            .ProjectTo<RamDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
