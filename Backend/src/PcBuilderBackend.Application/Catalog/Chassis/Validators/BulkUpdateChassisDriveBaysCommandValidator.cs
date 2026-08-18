using FluentValidation;
using PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkUpdateChassisDriveBays;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;

namespace PcBuilderBackend.Application.Catalog.Chassis.Validators;

public class BulkUpdateChassisDriveBaysCommandValidator
    : AbstractValidator<BulkUpdateChassisDriveBaysCommand>
{
    public BulkUpdateChassisDriveBaysCommandValidator()
    {
        RuleFor(x => x.ChassisId).NotEmpty();

        RuleFor(x => x.DriveBays)
            .Must(BeUniqueByFormFactor)
            .WithMessage("Duplicate drive bay form factors are not allowed.")
            .When(x => x.DriveBays.Count > 0);

        RuleForEach(x => x.DriveBays).ChildRules(bay =>
        {
            bay.RuleFor(b => b.FormFactor).IsInEnum();
            bay.RuleFor(b => b.SlotCount).GreaterThan(0);
        });
    }

    private static bool BeUniqueByFormFactor(List<ChassisDriveBayDto> bays) =>
        bays.GroupBy(x => x.FormFactor).All(g => g.Count() == 1);
}
