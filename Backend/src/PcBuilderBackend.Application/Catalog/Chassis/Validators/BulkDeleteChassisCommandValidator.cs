using FluentValidation;
using PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkDeleteChassis;

namespace PcBuilderBackend.Application.Catalog.Chassis.Validators;

public class BulkDeleteChassisCommandValidator : AbstractValidator<BulkDeleteChassisCommand>
{
    public BulkDeleteChassisCommandValidator()
    {
        RuleFor(x => x.Ids).NotEmpty();
        RuleForEach(x => x.Ids).NotEmpty();
    }
}
