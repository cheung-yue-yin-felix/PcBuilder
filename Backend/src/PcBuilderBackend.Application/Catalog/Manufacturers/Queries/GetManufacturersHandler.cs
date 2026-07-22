using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Manufacturers.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PcBuilderBackend.Application.Catalog.Manufacturers.Queries;

public class GetManufacturersHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetManufacturersQuery, List<ManufacturerDto>>
{
    public async Task<List<ManufacturerDto>> Handle(GetManufacturersQuery request, CancellationToken cancellationToken)
    {
        var entities = await context.Manufacturers
            .AsNoTracking()
            .Where(m => m.IsActive)
            .ToListAsync(cancellationToken);

        return mapper.Map<List<ManufacturerDto>>(entities);
    }
}