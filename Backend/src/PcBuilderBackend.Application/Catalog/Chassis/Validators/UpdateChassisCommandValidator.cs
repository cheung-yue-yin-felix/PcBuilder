using FluentValidation;
using PcBuilderBackend.Application.Catalog.Chassis.Commands.UpdateChassis;

namespace PcBuilderBackend.Application.Catalog.Chassis.Validators;

public class UpdateChassisCommandValidator : AbstractValidator<UpdateChassisCommand>
{
    public UpdateChassisCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        Include(new ChassisFieldsValidator<UpdateChassisCommand>());
    }
}
