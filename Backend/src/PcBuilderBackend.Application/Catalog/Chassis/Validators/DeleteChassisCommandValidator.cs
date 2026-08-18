using FluentValidation;
using PcBuilderBackend.Application.Catalog.Chassis.Commands.DeleteChassis;

namespace PcBuilderBackend.Application.Catalog.Chassis.Validators;

public class DeleteChassisCommandValidator : AbstractValidator<DeleteChassisCommand>
{
    public DeleteChassisCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
