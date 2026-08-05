using FluentValidation;
using PcBuilderBackend.Application.MasterData.Chipsets.Commands.UpdateChipset;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Validators;

public class UpdateChipsetCommandValidator : AbstractValidator<UpdateChipsetCommand>
{
    public UpdateChipsetCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.ManufacturerId)
            .NotEmpty().WithMessage("ManufacturerId is required.");

        RuleFor(x => x.SocketId)
            .NotEmpty().WithMessage("SocketId is required.");
    }
}