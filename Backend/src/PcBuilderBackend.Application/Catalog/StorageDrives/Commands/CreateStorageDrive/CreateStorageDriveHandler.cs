using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.StorageDrives.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Commands.CreateStorageDrive;

public class CreateStorageDriveHandler(
    IStorageDriveRepository storageDrives,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<CreateStorageDriveHandler> logger)
    : IRequestHandler<CreateStorageDriveCommand, StorageDriveDto>
{
    public async Task<StorageDriveDto> Handle(
        CreateStorageDriveCommand request,
        CancellationToken cancellationToken)
    {
        var entity = new StorageDrive(
            request.Name,
            request.ManufacturerId,
            request.Media,
            request.Interface,
            request.FormFactor,
            request.CapacityGb,
            request.PcieGeneration,
            request.Rpm);

        storageDrives.Add(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.Created(logger, EntityLog.StorageDrive, entity.Id);

        return mapper.Map<StorageDriveDto>(entity);
    }
}
