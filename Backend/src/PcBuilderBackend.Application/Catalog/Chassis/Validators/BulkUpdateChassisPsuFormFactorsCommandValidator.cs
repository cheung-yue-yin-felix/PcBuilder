using FluentValidation;
using PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkUpdateChassisPsuFormFactors;

namespace PcBuilderBackend.Application.Catalog.Chassis.Validators;

public class BulkUpdateChassisPsuFormFactorsCommandValidator
    : AbstractValidator<BulkUpdateChassisPsuFormFactorsCommand>
{
    public BulkUpdateChassisPsuFormFactorsCommandValidator()
    {
        RuleFor(x => x.ChassisId).NotEmpty();

        RuleFor(x => x.PsuFormFactors)
            .Must(values => values.Distinct().Count() == values.Count)
            .WithMessage("Duplicate PSU form factors are not allowed.")
            .When(x => x.PsuFormFactors.Count > 0);

        RuleForEach(x => x.PsuFormFactors).IsInEnum();
    }
}
