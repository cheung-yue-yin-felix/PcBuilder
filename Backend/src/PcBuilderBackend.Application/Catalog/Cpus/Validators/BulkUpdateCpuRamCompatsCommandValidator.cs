using FluentValidation;
using PcBuilderBackend.Application.Catalog.Cpus.Commands.BulkUpdateCpuRamCompats;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;

namespace PcBuilderBackend.Application.Catalog.Cpus.Validators;

public class BulkUpdateCpuRamCompatsCommandValidator : AbstractValidator<BulkUpdateCpuRamCompatsCommand>
{
    public BulkUpdateCpuRamCompatsCommandValidator()
    {
        RuleFor(x => x.CpuId)
            .NotEmpty().WithMessage("CpuId is required");

        RuleFor(x => x.RamCompats)
            .Must(BeUniqueByKey)
            .WithMessage("Duplicate RAM compatibility entries are not allowed.")
            .When(x => x.RamCompats.Count > 0);

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
    }

    private static bool BeUniqueByKey(List<CpuRamCompatDto> compats)
    {
        return compats
            .GroupBy(x => (x.DdrGeneration, x.RamModuleCount, x.RamRank))
            .All(g => g.Count() == 1);
    }
}
