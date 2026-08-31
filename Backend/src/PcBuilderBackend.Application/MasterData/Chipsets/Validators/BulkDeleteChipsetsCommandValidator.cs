using FluentValidation;
using PcBuilderBackend.Application.MasterData.Chipsets.Commands.BulkDeleteChipsets;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Validators;

public class BulkDeleteChipsetsCommandValidator: AbstractValidator<BulkDeleteChipsetsCommand>
{
    public BulkDeleteChipsetsCommandValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithMessage("Ids list cannot be empty.");
    }
}