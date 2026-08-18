using FluentValidation;
using PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkUpdateChassisRadiators;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;

namespace PcBuilderBackend.Application.Catalog.Chassis.Validators;

public class BulkUpdateChassisRadiatorsCommandValidator
    : AbstractValidator<BulkUpdateChassisRadiatorsCommand>
{
    public BulkUpdateChassisRadiatorsCommandValidator()
    {
        RuleFor(x => x.ChassisId).NotEmpty();

        RuleFor(x => x.Radiators)
            .Must(BeUniqueByKey)
            .WithMessage("Duplicate radiators (same Length and Location) are not allowed.")
            .When(x => x.Radiators.Count > 0);

        RuleForEach(x => x.Radiators).ChildRules(radiator =>
        {
            radiator.RuleFor(r => r.Length).IsInEnum();
            radiator.RuleFor(r => r.Location).IsInEnum();
            radiator.RuleFor(r => r.RadiatorCount).GreaterThan(0);
        });
    }

    private static bool BeUniqueByKey(List<ChassisRadiatorDto> radiators) =>
        radiators.GroupBy(x => (x.Length, x.Location)).All(g => g.Count() == 1);
}
