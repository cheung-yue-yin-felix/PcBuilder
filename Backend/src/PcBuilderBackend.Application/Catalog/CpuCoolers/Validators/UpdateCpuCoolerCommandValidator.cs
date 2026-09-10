using FluentValidation;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Commands.UpdateCpuCooler;

namespace PcBuilderBackend.Application.Catalog.CpuCoolers.Validators;

public class UpdateCpuCoolerCommandValidator : AbstractValidator<UpdateCpuCoolerCommand>
{
    public UpdateCpuCoolerCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
        Include(new CpuCoolerFieldsValidator<UpdateCpuCoolerCommand>());
    }
}
