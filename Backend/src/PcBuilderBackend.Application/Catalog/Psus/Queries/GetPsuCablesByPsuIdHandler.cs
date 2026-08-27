using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Psus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Psus.Queries;

public class GetPsuCablesByPsuIdHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetPsuCablesByPsuIdQuery, List<PsuCableDto>>
{
    public async Task<List<PsuCableDto>> Handle(
        GetPsuCablesByPsuIdQuery request,
        CancellationToken cancellationToken)
    {
        var psu = await context.Psus
            .AsNoTracking()
            .Include(x => x.Cables)
            .FirstOrDefaultAsync(x => x.Id == request.PsuId && x.IsActive, cancellationToken);

        if (psu is null)
            return [];

        return mapper.Map<List<PsuCableDto>>(
            psu.Cables.Where(x => x.IsActive).OrderBy(x => x.Type).ToList());
    }
}
