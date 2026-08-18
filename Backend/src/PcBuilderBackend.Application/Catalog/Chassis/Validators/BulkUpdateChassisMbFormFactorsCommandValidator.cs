using FluentValidation;
using PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkUpdateChassisMbFormFactors;

namespace PcBuilderBackend.Application.Catalog.Chassis.Validators;

public class BulkUpdateChassisMbFormFactorsCommandValidator
    : AbstractValidator<BulkUpdateChassisMbFormFactorsCommand>
{
    public BulkUpdateChassisMbFormFactorsCommandValidator()
    {
        RuleFor(x => x.ChassisId).NotEmpty();

        RuleFor(x => x.MbFormFactors)
            .Must(values => values.Distinct().Count() == values.Count)
            .WithMessage("Duplicate motherboard form factors are not allowed.")
            .When(x => x.MbFormFactors.Count > 0);

        RuleForEach(x => x.MbFormFactors).IsInEnum();
    }
}
