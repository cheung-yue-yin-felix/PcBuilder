using FluentValidation;
using PcBuilderBackend.Application.MasterData.Gpus.Commands.DeleteGpu;

namespace PcBuilderBackend.Application.MasterData.Gpus.Validators;

public class DeleteGpuCommandValidator: AbstractValidator<DeleteGpuCommand>
{
    public DeleteGpuCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.");
    }
}