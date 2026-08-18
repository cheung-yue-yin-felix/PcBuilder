using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Memories.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Memories.Queries;

public class GetMemoryByIdHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetMemoryByIdQuery, RamDto?>
{
    public async Task<RamDto?> Handle(GetMemoryByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await context.Rams
            .AsNoTracking()
            .Include(x => x.Manufacturer)
            .FirstOrDefaultAsync(r => r.Id == request.Id && r.IsActive, cancellationToken);
        return entity == null ? null : mapper.Map<RamDto>(entity);
    }
}
