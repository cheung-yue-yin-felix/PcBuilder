using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Commands.BulkUpdateMotherboardUsbPorts;

public class BulkUpdateMotherboardUsbPortsHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ILogger<BulkUpdateMotherboardUsbPortsHandler> logger)
    : IRequestHandler<BulkUpdateMotherboardUsbPortsCommand, List<MotherboardUsbDto>?>
{
    public async Task<List<MotherboardUsbDto>?> Handle(
        BulkUpdateMotherboardUsbPortsCommand request,
        CancellationToken cancellationToken)
    {
        var motherboard = await context.Motherboards
            .Include(x => x.UsbPorts)
            .FirstOrDefaultAsync(x => x.Id == request.MotherboardId && x.IsActive, cancellationToken);

        if (motherboard is null)
        {
            EntityLog.NotFound(logger, EntityLog.Motherboard, request.MotherboardId);
            return null;
        }

        var existingByKey = motherboard.UsbPorts.ToDictionary(x => (x.UsbType, x.UsbVersion));
        var touchedKeys = new HashSet<(UsbType, UsbVersion)>();

        foreach (var port in request.UsbPorts)
        {
            var key = (port.UsbType, port.UsbVersion);
            touchedKeys.Add(key);

            if (existingByKey.TryGetValue(key, out var existing))
            {
                existing.UpdateSpecs(port.UsbVersion, port.UsbType, port.PortCount);
            }
            else
            {
                motherboard.AddUsbPort(new MotherboardUsb(
                    request.MotherboardId,
                    port.UsbVersion,
                    port.UsbType,
                    port.PortCount));
            }
        }

        foreach (var existing in existingByKey.Values.Where(x =>
                     !touchedKeys.Contains((x.UsbType, x.UsbVersion))))
        {
            motherboard.RemoveUsbPort(existing);
        }

        await context.SaveChangesAsync(cancellationToken);

        EntityLog.MotherboardUsbPortsUpdated(logger, motherboard.Id);

        return [.. motherboard.UsbPorts.Select(mapper.Map<MotherboardUsbDto>)];
    }
}
