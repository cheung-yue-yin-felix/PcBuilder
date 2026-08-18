using FluentValidation;
using PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkUpdateChassisFanMounts;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;

namespace PcBuilderBackend.Application.Catalog.Chassis.Validators;

public class BulkUpdateChassisFanMountsCommandValidator
    : AbstractValidator<BulkUpdateChassisFanMountsCommand>
{
    public BulkUpdateChassisFanMountsCommandValidator()
    {
        RuleFor(x => x.ChassisId).NotEmpty();

        RuleFor(x => x.FanMounts)
            .Must(BeUniqueByLocation)
            .WithMessage("Duplicate fan mount locations are not allowed.")
            .When(x => x.FanMounts.Count > 0);

        RuleForEach(x => x.FanMounts).ChildRules(mount =>
        {
            mount.RuleFor(m => m.Location).IsInEnum();
            mount.RuleFor(m => m.Options)
                .Must(BeUniqueByDiameter)
                .WithMessage("Duplicate fan mount option diameters are not allowed.")
                .When(m => m.Options.Count > 0);

            mount.RuleForEach(m => m.Options).ChildRules(option =>
            {
                option.RuleFor(o => o.Diameter).IsInEnum();
                option.RuleFor(o => o.SlotCount).GreaterThan(0);
            });
        });
    }

    private static bool BeUniqueByLocation(List<ChassisFanMountDto> mounts) =>
        mounts.GroupBy(x => x.Location).All(g => g.Count() == 1);

    private static bool BeUniqueByDiameter(List<ChassisFanMountOptionDto> options) =>
        options.GroupBy(x => x.Diameter).All(g => g.Count() == 1);
}
