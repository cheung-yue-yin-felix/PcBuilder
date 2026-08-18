using FluentValidation;
using PcBuilderBackend.Application.Catalog.Cpus.Commands.DeleteCpu;

namespace PcBuilderBackend.Application.Catalog.Cpus.Validators;

public class DeleteCpuCommandValidator: AbstractValidator<DeleteCpuCommand>
{
    public DeleteCpuCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}
