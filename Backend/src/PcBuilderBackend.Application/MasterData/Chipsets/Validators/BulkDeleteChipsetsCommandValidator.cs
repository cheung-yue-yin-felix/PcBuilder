using FluentValidation;
using PcBuilderBackend.Application.MasterData.Chipsets.Commands.BulkDeleteChipsets;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Validators;

public class BulkDeleteChipsetsCommandValidator: AbstractValidator<BulkDeleteChipsetsCommand>
{
    public BulkDeleteChipsetsCommandValidator()
    {
        RuleFor(x => x.ChipsetIds).NotEmpty().WithMessage("ChipsetIds list cannot be empty.");
    }
}