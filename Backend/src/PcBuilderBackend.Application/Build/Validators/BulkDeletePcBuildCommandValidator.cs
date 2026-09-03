using FluentValidation;
using PcBuilderBackend.Application.Build.Commands.BulkDeletePcBuild;

namespace PcBuilderBackend.Application.Build.Validators;

public class BulkDeletePcBuildCommandValidator : AbstractValidator<BulkDeletePcBuildCommand>
{
    public BulkDeletePcBuildCommandValidator()
    {
        RuleFor(x => x.Ids).NotEmpty();
        RuleForEach(x => x.Ids).NotEmpty();
    }
}
