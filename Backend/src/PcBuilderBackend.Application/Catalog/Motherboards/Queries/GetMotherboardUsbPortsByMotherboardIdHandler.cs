using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Queries;

public class GetMotherboardUsbPortsByMotherboardIdHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetMotherboardUsbPortsByMotherboardIdQuery, List<MotherboardUsbDto>>
{
    public async Task<List<MotherboardUsbDto>> Handle(
        GetMotherboardUsbPortsByMotherboardIdQuery request,
        CancellationToken cancellationToken)
    {
        var motherboard = await context.Motherboards
            .AsNoTracking()
            .Include(m => m.UsbPorts)
            .FirstOrDefaultAsync(m => m.Id == request.MotherboardId && m.IsActive, cancellationToken);

        if (motherboard is null)
            return [];

        var ports = motherboard.UsbPorts
            .Where(p => p.IsActive)
            .OrderBy(p => p.UsbType)
            .ThenBy(p => p.UsbVersion)
            .ToList();

        return mapper.Map<List<MotherboardUsbDto>>(ports);
    }
}
