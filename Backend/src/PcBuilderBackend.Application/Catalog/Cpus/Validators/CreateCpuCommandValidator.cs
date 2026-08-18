using FluentValidation;
using PcBuilderBackend.Application.Catalog.Cpus.Commands.CreateCpu;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;

namespace PcBuilderBackend.Application.Catalog.Cpus.Validators;

public class CreateCpuCommandValidator : AbstractValidator<CreateCpuCommand>
{
    public CreateCpuCommandValidator()
    {
        RuleFor(x => x.ManufacturerId)
            .NotEmpty().WithMessage("ManufacturerId is required");

        RuleFor(x => x.SocketId)
            .NotEmpty().WithMessage("SocketId is required");

        RuleFor(x => x.SeriesId)
            .NotEmpty().WithMessage("SeriesId is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200);

        RuleFor(x => x.MaxMemoryGb)
            .GreaterThan(0).WithMessage("MaxMemoryGb must be greater than 0");

        RuleFor(x => x.ThermalDesignPower)
            .GreaterThan(0).WithMessage("ThermalDesignPower must be greater than 0");

        RuleFor(x => x.PowerConsumptionWatts)
            .GreaterThan(0).WithMessage("PowerConsumptionWatts must be greater than 0");

        RuleFor(x => x.RamCompats)
            .NotEmpty().WithMessage("At least one CPU RAM compatibility entry is required");

        RuleForEach(x => x.RamCompats).ChildRules(compat =>
        {
            compat.RuleFor(x => x.DdrGeneration)
                .IsInEnum().WithMessage("DdrGeneration is invalid");

            compat.RuleFor(x => x.RamModuleCount)
                .GreaterThan(0).WithMessage("RamModuleCount must be greater than 0");

            compat.RuleFor(x => x.MaxSpeedMts)
                .GreaterThan(0).WithMessage("MaxSpeedMts must be greater than 0");

            compat.RuleFor(x => x.RamRank)
                .IsInEnum().WithMessage("RamRank is invalid");
        });

        RuleFor(x => x.SupportChipsets)
            .NotEmpty().WithMessage("At least one supported chipset entry is required")
            .Must(BeUniqueChipsetIds)
            .WithMessage("Duplicate chipset support entries are not allowed.");

        RuleForEach(x => x.SupportChipsets).ChildRules(support =>
        {
            support.RuleFor(x => x.ChipsetId)
                .NotEmpty().WithMessage("ChipsetId is required");
        });
    }

    private static bool BeUniqueChipsetIds(List<CpuSupportChipsetDto> supports)
    {
        return supports.GroupBy(x => x.ChipsetId).All(g => g.Count() == 1);
    }
}
