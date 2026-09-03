using MediatR;
using PcBuilderBackend.Application.Build;
using PcBuilderBackend.Application.Build.Dto;

namespace PcBuilderBackend.Application.Build.Queries;

public class CheckPcBuildCompatibilityHandler(ICompatibilityChecker checker)
    : IRequestHandler<CheckPcBuildCompatibilityQuery, CompatibilityCheckDto>
{
    public async Task<CompatibilityCheckDto> Handle(
        CheckPcBuildCompatibilityQuery query,
        CancellationToken cancellationToken)
    {
        var checks = await checker.CheckCompatibilityAsync(
            query.ChassisId,
            query.MotherboardId,
            query.CpuId,
            query.CpuCoolerId,
            query.RamKitId,
            query.GraphicsCardId,
            query.PsuId,
            query.ChassisFans,
            query.StorageDevices,
            query.WiredNetworkAdapters,
            query.WirelessNetworkAdapters);

        return CompatibilityCheckDto.From(checks);
    }
}
