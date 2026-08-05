using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Memories.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Memories.Queries;

public class GetMemoriesByMotherboardIdHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetMemoriesByMotherboardIdQuery, List<RamDto>>
{
    public async Task<List<RamDto>> Handle(GetMemoriesByMotherboardIdQuery request, CancellationToken cancellationToken)
    {
        var motherboard = await context.Motherboards.AsNoTracking().FirstOrDefaultAsync(m => m.Id == request.MotherboardId && m.IsActive, cancellationToken);

        if (motherboard is null) return new List<RamDto>();
        
        return await context.Rams
            .AsNoTracking()
            .Where(x => x.IsActive)
            .Where(x => motherboard.CheckMemoryCompatibility(x))
            .ProjectTo<RamDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }


}