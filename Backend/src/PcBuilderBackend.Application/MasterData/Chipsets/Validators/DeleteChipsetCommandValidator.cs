using FluentValidation;
using PcBuilderBackend.Application.MasterData.Chipsets.Commands.DeleteChipset;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Validators;

public class DeleteChipsetCommandValidator : AbstractValidator<DeleteChipsetCommand>
{
    public DeleteChipsetCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("ChipsetId is required.");
    }
}