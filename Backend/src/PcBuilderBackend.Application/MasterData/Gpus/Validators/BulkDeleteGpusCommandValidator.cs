using FluentValidation;
using PcBuilderBackend.Application.MasterData.Gpus.Commands.BulkDeleteGpus;

namespace PcBuilderBackend.Application.MasterData.Gpus.Validators;

public class BulkDeleteGpusCommandValidator: AbstractValidator<BulkDeleteGpusCommand>
{
    public BulkDeleteGpusCommandValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithMessage("Ids list cannot be empty.");
    }
}