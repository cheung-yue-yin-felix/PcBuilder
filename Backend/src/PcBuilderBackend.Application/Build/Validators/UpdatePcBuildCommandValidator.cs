using FluentValidation;
using PcBuilderBackend.Application.Build.Commands.UpdatePcBuild;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Build.Validators;

public class UpdatePcBuildCommandValidator : AbstractValidator<UpdatePcBuildCommand>
{
    public UpdatePcBuildCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.ChassisId).NotEmpty();
        RuleFor(x => x.MotherboardId).NotEmpty();
        RuleFor(x => x.CpuId).NotEmpty();
        RuleFor(x => x.RamKitId).NotEmpty();
        RuleFor(x => x.PsuId).NotEmpty();
        RuleFor(x => x.ChassisFans).NotNull();
        RuleFor(x => x.StorageDevices).NotNull();
        RuleFor(x => x.WiredNetworkAdapters).NotNull();
        RuleFor(x => x.WirelessNetworkAdapters).NotNull();
        PcBuildPartRules.Parts(RuleForEach(x => x.ChassisFans), PcBuildPartType.ChassisFan);
        PcBuildPartRules.Parts(RuleForEach(x => x.StorageDevices), PcBuildPartType.StorageDrive);
        PcBuildPartRules.Parts(RuleForEach(x => x.WiredNetworkAdapters), PcBuildPartType.WiredNetworkAdapter);
        PcBuildPartRules.Parts(RuleForEach(x => x.WirelessNetworkAdapters), PcBuildPartType.WirelessNetworkAdapter);
    }
}
