using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Build.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Build.Commands.UpdatePcBuild;

public class UpdatePcBuildHandler(
    IPcBuildRepository pcBuilds, 
    IUnitOfWork unitOfWork, 
    ICompatibilityChecker checker, 
    ILogger<UpdatePcBuildHandler> logger,
    ICurrentUser currentUser,
    IMapper mapper) : IRequestHandler<UpdatePcBuildCommand, PcBuildDto?>
{
    public async Task<PcBuildDto?> Handle(UpdatePcBuildCommand command, CancellationToken cancellationToken)
    {
        var pcBuild = await pcBuilds.GetWithChildrenAsync(command.Id);

        if (pcBuild is null)
        {
            EntityLog.NotFound(logger, EntityLog.PcBuild, command.Id);
            return null;
        }

        if (pcBuild.User is null || !currentUser.UserId.HasValue || pcBuild.User.UserId != currentUser.UserId.Value)
        {
            EntityLog.NotAuthorized(logger, EntityLog.PcBuild, command.Id);
            return null;
        }

        var compatibilityResults = await checker.CheckCompatibilityAsync(
            command.ChassisId,
            command.MotherboardId,
            command.CpuId,
            command.CpuCoolerId,
            command.RamKitId,
            command.GraphicsCardId,
            command.PsuId,
            command.ChassisFans,
            command.StorageDevices,
            command.WiredNetworkAdapters,
            command.WirelessNetworkAdapters
        );

        if (compatibilityResults.Any(r => r.Result.Status == PartsCompatibility.Incompatible))
        {
            EntityLog.CompatibilityCheckFailed(logger, pcBuild.Id);
            return null;
        }

        pcBuild.Update(
            command.Name,
            command.Description,
            command.ChassisId,
            command.MotherboardId,
            command.CpuId,
            command.CpuCoolerId,
            command.RamKitId,
            command.GraphicsCardId,
            command.PsuId
        );

        foreach (var chassisFan in pcBuild.ChassisFans)
        {
            pcBuild.RemoveChassisFan(chassisFan.PartId);
            pcBuilds.DeletePart(chassisFan);
        }

        foreach (var chassisFan in command.ChassisFans)
        {
            pcBuild.AddChassisFan(chassisFan.PartId, chassisFan.Quantity);
        }

        foreach (var storageDevice in pcBuild.StorageDevices)
        {
            pcBuild.RemoveStorageDevice(storageDevice.PartId, storageDevice.Quantity);
            pcBuilds.DeletePart(storageDevice);
        }

        foreach (var storageDevice in command.StorageDevices)
        {
            pcBuild.AddStorageDevice(storageDevice.PartId, storageDevice.Quantity);
        }

        foreach (var wiredNetworkAdapter in pcBuild.WiredNetworkAdapters)
        {
            pcBuild.RemoveWiredNetworkAdapter(wiredNetworkAdapter.PartId, wiredNetworkAdapter.Quantity);
            pcBuilds.DeletePart(wiredNetworkAdapter);
        }

        foreach (var wiredNetworkAdapter in command.WiredNetworkAdapters)
        {
            pcBuild.AddWiredNetworkAdapter(wiredNetworkAdapter.PartId, wiredNetworkAdapter.Quantity);
        }

        foreach (var wirelessNetworkAdapter in pcBuild.WirelessNetworkAdapters)
        {
            pcBuild.RemoveWirelessNetworkAdapter(wirelessNetworkAdapter.PartId, wirelessNetworkAdapter.Quantity);
            pcBuilds.DeletePart(wirelessNetworkAdapter);
        }

        foreach (var wirelessNetworkAdapter in command.WirelessNetworkAdapters)
        {
            pcBuild.AddWirelessNetworkAdapter(wirelessNetworkAdapter.PartId, wirelessNetworkAdapter.Quantity);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.Updated(logger, EntityLog.PcBuild, pcBuild.Id);
        return mapper.Map<PcBuildDto>(pcBuild);
    }
}