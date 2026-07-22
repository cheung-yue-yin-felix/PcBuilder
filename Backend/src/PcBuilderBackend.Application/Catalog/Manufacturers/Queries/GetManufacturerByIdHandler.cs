using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Manufacturers.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace PcBuilderBackend.Application.Catalog.Manufacturers.Queries;

public class GetManufacturerByIdHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetManufacturerByIdQuery, ManufacturerDto?>
{
    public async Task<ManufacturerDto?> Handle(GetManufacturerByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await context.Manufacturers.AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == request.Id && m.IsActive, cancellationToken);

        return entity is null ? null : mapper.Map<ManufacturerDto>(entity);
    }
}