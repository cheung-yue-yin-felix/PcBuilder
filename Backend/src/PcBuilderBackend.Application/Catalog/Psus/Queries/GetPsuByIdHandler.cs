using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Psus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Psus.Queries;

public class GetPsuByIdHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetPsuByIdQuery, PsuDto?>
{
    public async Task<PsuDto?> Handle(GetPsuByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await context.Psus
            .AsNoTracking()
            .Include(x => x.Manufacturer)
            .Include(x => x.Cables)
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, cancellationToken);

        return entity is null ? null : mapper.Map<PsuDto>(entity);
    }
}
