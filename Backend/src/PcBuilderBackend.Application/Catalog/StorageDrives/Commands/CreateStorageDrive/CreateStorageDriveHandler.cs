using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.StorageDrives.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.ValueObjects;

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
            new StorageDriveSpecs
            {
                Media = request.Media,
                Interface = request.Interface,
                FormFactor = request.FormFactor,
                CapacityGb = request.CapacityGb,
                PcieGeneration = request.PcieGeneration,
                Rpm = request.Rpm
            });

        storageDrives.Add(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.Created(logger, EntityLog.StorageDrive, entity.Id);

        return mapper.Map<StorageDriveDto>(entity);
    }
}
