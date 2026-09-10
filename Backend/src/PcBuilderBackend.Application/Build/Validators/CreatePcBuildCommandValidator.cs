using FluentValidation;
using PcBuilderBackend.Application.Build.Commands.CreatePcBuild;

namespace PcBuilderBackend.Application.Build.Validators;

public class CreatePcBuildCommandValidator : AbstractValidator<CreatePcBuildCommand>
{
    public CreatePcBuildCommandValidator()
    {
        Include(new PcBuildFieldsValidator<CreatePcBuildCommand>());
    }
}
