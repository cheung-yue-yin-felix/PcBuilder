using FluentValidation;
using PcBuilderBackend.Application.MasterData.Gpus.Commands.BulkDeleteGpus;

namespace PcBuilderBackend.Application.MasterData.Gpus.Validators;

public class BulkDeleteGpusCommandValidator: AbstractValidator<BulkDeleteGpusCommand>
{
    public BulkDeleteGpusCommandValidator()
    {
        RuleFor(x => x.GpuIds).NotEmpty().WithMessage("GpuIds list cannot be empty.");
    }
}