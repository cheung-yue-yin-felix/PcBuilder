using FluentValidation;
using PcBuilderBackend.Application.Build.Commands.UpdatePcBuild;

namespace PcBuilderBackend.Application.Build.Validators;

public class UpdatePcBuildCommandValidator : AbstractValidator<UpdatePcBuildCommand>
{
    public UpdatePcBuildCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        Include(new PcBuildFieldsValidator<UpdatePcBuildCommand>());
    }
}
