using FluentValidation;
using PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkUpdateChassis;

namespace PcBuilderBackend.Application.Catalog.Chassis.Validators;

public class BulkUpdateChassisCommandValidator : AbstractValidator<BulkUpdateChassisCommand>
{
    public BulkUpdateChassisCommandValidator()
    {
        RuleFor(x => x.Items).NotEmpty();
        RuleForEach(x => x.Items).SetValidator(new UpdateChassisCommandValidator());
    }
}
