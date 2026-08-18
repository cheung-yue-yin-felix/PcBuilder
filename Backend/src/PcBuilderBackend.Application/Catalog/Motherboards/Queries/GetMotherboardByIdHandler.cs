using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Queries;

public class GetMotherboardByIdHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetMotherboardByIdQuery, MotherboardDto?>
{
    public async Task<MotherboardDto?> Handle(GetMotherboardByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await context.Motherboards
            .AsNoTracking()
            .Include(m => m.PcieSlots)
            .Include(m => m.M2Slots)
            .ThenInclude(m => m.FormFactors)
            .Include(m => m.UsbPorts)
            .FirstOrDefaultAsync(m => m.Id == request.Id && m.IsActive, cancellationToken);

        return entity == null ? null : mapper.Map<MotherboardDto>(entity);
    }
}
