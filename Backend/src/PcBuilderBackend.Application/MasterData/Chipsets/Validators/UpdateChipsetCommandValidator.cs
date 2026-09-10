using FluentValidation;
using PcBuilderBackend.Application.MasterData.Chipsets.Commands.UpdateChipset;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Validators;

public class UpdateChipsetCommandValidator : AbstractValidator<UpdateChipsetCommand>
{
    public UpdateChipsetCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
        Include(new ChipsetFieldsValidator<UpdateChipsetCommand>());
    }
}
