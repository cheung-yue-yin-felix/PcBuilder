using FluentValidation;
using PcBuilderBackend.Application.Catalog.Cpus.Commands.BulkUpdateCpuSupportChipsets;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;

namespace PcBuilderBackend.Application.Catalog.Cpus.Validators;

public class BulkUpdateCpuSupportChipsetsCommandValidator
    : AbstractValidator<BulkUpdateCpuSupportChipsetsCommand>
{
    public BulkUpdateCpuSupportChipsetsCommandValidator()
    {
        RuleFor(x => x.CpuId)
            .NotEmpty().WithMessage("CpuId is required");

        RuleFor(x => x.SupportChipsets)
            .Must(BeUniqueByChipsetId)
            .WithMessage("Duplicate chipset support entries are not allowed.")
            .When(x => x.SupportChipsets.Count > 0);

        RuleForEach(x => x.SupportChipsets).ChildRules(support =>
        {
            support.RuleFor(x => x.ChipsetId)
                .NotEmpty().WithMessage("ChipsetId is required");
        });
    }

    private static bool BeUniqueByChipsetId(List<CpuSupportChipsetDto> supports)
    {
        return supports.GroupBy(x => x.ChipsetId).All(g => g.Count() == 1);
    }
}
