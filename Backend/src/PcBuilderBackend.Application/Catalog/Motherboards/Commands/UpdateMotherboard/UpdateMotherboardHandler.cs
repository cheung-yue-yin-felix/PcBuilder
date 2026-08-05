using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Commands.UpdateMotherboard;

public class UpdateMotherboardHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<UpdateMotherboardCommand, MotherboardDto?>
{
    public async Task<MotherboardDto?> Handle(UpdateMotherboardCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Motherboards
            .FirstOrDefaultAsync(m => m.Id == request.Id && m.IsActive, cancellationToken);
        if (entity == null) return null;

        entity.Rename(request.Name);
        entity.UpdateManufacturer(request.ManufacturerId);
        entity.UpdateSpecs(
            request.SocketId,
            request.ChipsetId,
            request.RamSlots,
            request.MaxMemoryGb,
            request.MaxDimmSizeGb,
            request.SataPorts,
            request.FanConnectors,
            request.EpsConnectors,
            request.WidthMm,
            request.HeightMm,
            request.DdrGeneration,
            request.RamFormFactor,
            request.FormFactor,
            request.WifiEnabled,
            request.BluetoothEnabled);

        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<MotherboardDto>(entity);
    }
}
