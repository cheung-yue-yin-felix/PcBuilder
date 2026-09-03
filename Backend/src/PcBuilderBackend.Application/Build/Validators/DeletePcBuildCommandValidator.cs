using FluentValidation;
using PcBuilderBackend.Application.Build.Commands.DeletePcBuild;

namespace PcBuilderBackend.Application.Build.Validators;

public class DeletePcBuildCommandValidator : AbstractValidator<DeletePcBuildCommand>
{
    public DeletePcBuildCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
