using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Commands.CreateWiredNetworkAdapter;

public class CreateWiredNetworkAdapterHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ILogger<CreateWiredNetworkAdapterHandler> logger)
    : IRequestHandler<CreateWiredNetworkAdapterCommand, WiredNetworkAdapterDto>
{
    public async Task<WiredNetworkAdapterDto> Handle(
        CreateWiredNetworkAdapterCommand request,
        CancellationToken cancellationToken)
    {
        var entity = new WiredNetworkAdapter(
            request.Name,
            request.ManufacturerId,
            request.HostInterface,
            request.MaxSpeedMbps,
            request.UsbVersion,
            request.UsbType,
            request.PcieSlotType);

        context.WiredNetworkAdapters.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        EntityLog.Created(logger, EntityLog.WiredNetworkAdapter, entity.Id);

        return mapper.Map<WiredNetworkAdapterDto>(entity);
    }
}
