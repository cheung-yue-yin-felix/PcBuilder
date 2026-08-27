using FluentValidation;
using PcBuilderBackend.Application.Catalog.ChassisFans.Commands.DeleteChassisFan;

namespace PcBuilderBackend.Application.Catalog.ChassisFans.Validators;

public class DeleteChassisFanCommandValidator : AbstractValidator<DeleteChassisFanCommand>
{
    public DeleteChassisFanCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
