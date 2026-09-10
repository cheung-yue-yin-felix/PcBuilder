using FluentValidation;
using PcBuilderBackend.Application.MasterData.Gpus.Commands.UpdateGpu;

namespace PcBuilderBackend.Application.MasterData.Gpus.Validators;

public class UpdateGpuCommandValidator : AbstractValidator<UpdateGpuCommand>
{
    public UpdateGpuCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.");
        Include(new GpuFieldsValidator<UpdateGpuCommand>());
    }
}
