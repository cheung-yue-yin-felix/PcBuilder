using FluentValidation;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Commands.DeleteCpuCooler;

namespace PcBuilderBackend.Application.Catalog.CpuCoolers.Validators;

public class DeleteCpuCoolerCommandValidator : AbstractValidator<DeleteCpuCoolerCommand>
{
    public DeleteCpuCoolerCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
    }
}
