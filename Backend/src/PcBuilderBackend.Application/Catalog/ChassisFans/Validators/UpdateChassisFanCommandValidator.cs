using FluentValidation;
using PcBuilderBackend.Application.Catalog.ChassisFans.Commands.UpdateChassisFan;

namespace PcBuilderBackend.Application.Catalog.ChassisFans.Validators;

public class UpdateChassisFanCommandValidator : AbstractValidator<UpdateChassisFanCommand>
{
    public UpdateChassisFanCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        Include(new ChassisFanFieldsValidator<UpdateChassisFanCommand>());
    }
}
