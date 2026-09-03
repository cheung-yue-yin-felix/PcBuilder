using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Build;
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

        if (pcBuild.User is null || currentUser.UserId is not { } userId || pcBuild.User.UserId != userId)
        {
            EntityLog.NotAuthorized(logger, EntityLog.PcBuild, command.Id);
            throw new UnauthorizedAccessException();
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
            command.WirelessNetworkAdapters);

        if (compatibilityResults.Any(r => r.Result.Status == PartsCompatibility.Incompatible))
        {
            EntityLog.CompatibilityCheckFailed(logger, pcBuild.Id);
            throw new ArgumentException("Build is incompatible.");
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
            command.PsuId);

        pcBuild.User.Update(pcBuild.Id, userId, command.IsPublic);

        foreach (var part in pcBuild.Parts.ToList())
        {
            switch (part.Type)
            {
                case PcBuildPartType.ChassisFan:
                    pcBuild.RemoveChassisFan(part.PartId, part.Quantity);
                    break;
                case PcBuildPartType.StorageDrive:
                    pcBuild.RemoveStorageDevice(part.PartId, part.Quantity);
                    break;
                case PcBuildPartType.WiredNetworkAdapter:
                    pcBuild.RemoveWiredNetworkAdapter(part.PartId, part.Quantity);
                    break;
                case PcBuildPartType.WirelessNetworkAdapter:
                    pcBuild.RemoveWirelessNetworkAdapter(part.PartId, part.Quantity);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(part.Type), part.Type, null);
            }

            pcBuilds.DeletePart(part);
        }

        foreach (var chassisFan in command.ChassisFans ?? [])
            pcBuild.AddChassisFan(chassisFan.PartId, chassisFan.Quantity);

        foreach (var storageDevice in command.StorageDevices ?? [])
            pcBuild.AddStorageDevice(storageDevice.PartId, storageDevice.Quantity);

        foreach (var wiredNetworkAdapter in command.WiredNetworkAdapters ?? [])
            pcBuild.AddWiredNetworkAdapter(wiredNetworkAdapter.PartId, wiredNetworkAdapter.Quantity);

        foreach (var wirelessNetworkAdapter in command.WirelessNetworkAdapters ?? [])
            pcBuild.AddWirelessNetworkAdapter(wirelessNetworkAdapter.PartId, wirelessNetworkAdapter.Quantity);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.Updated(logger, EntityLog.PcBuild, pcBuild.Id);
        return mapper.Map<PcBuildDto>(pcBuild);
    }
}
