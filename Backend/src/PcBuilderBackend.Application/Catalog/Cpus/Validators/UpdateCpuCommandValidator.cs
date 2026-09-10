using FluentValidation;
using PcBuilderBackend.Application.Catalog.Cpus.Commands.UpdateCpu;

namespace PcBuilderBackend.Application.Catalog.Cpus.Validators;

public class UpdateCpuCommandValidator : AbstractValidator<UpdateCpuCommand>
{
    public UpdateCpuCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
        Include(new CpuFieldsValidator<UpdateCpuCommand>());
    }
}
