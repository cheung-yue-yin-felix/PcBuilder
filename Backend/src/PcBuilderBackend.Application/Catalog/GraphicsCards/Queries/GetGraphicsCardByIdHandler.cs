using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Queries;

public class GetGraphicsCardByIdHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetGraphicsCardByIdQuery, GraphicsCardDto?>
{
    public async Task<GraphicsCardDto?> Handle(
        GetGraphicsCardByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await context.GraphicsCards
            .AsNoTracking()
            .Where(x => x.Id == request.Id && x.IsActive)
            .ProjectTo<GraphicsCardDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
