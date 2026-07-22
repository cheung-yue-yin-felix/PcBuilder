using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Cpus.Queries;

public class GetCpuHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetCpuQuery, List<CpuDto>>
{
    public async Task<List<CpuDto>> Handle(GetCpuQuery request, CancellationToken cancellationToken)
    {
        var entities = context.Cpus.AsNoTracking().Where(c => c.IsActive);

        if (request.ManufacturerId != Guid.Empty)
            entities = entities.Where(x => x.ManufacturerId == request.ManufacturerId);

        if (request.SocketId != Guid.Empty)
            entities = entities.Where(x => x.SocketId == request.SocketId);
        
        if (request.SeriesId != Guid.Empty)
            entities = entities.Where(x => x.SeriesId == request.SeriesId);

        if (request.DdrGeneration.HasValue && Enum.IsDefined(request.DdrGeneration.Value))
        {
            var ddr = request.DdrGeneration.Value;
            entities = entities.Where(x => x.DdrGeneration == ddr);
        }

        if (!string.IsNullOrWhiteSpace(request.Name))
            entities = entities.Where(x => x.Name.Contains(request.Name, StringComparison.CurrentCultureIgnoreCase));

        var list = await entities
            .OrderBy(x => x.Name)
            .ProjectTo<CpuDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return list;
    }
}