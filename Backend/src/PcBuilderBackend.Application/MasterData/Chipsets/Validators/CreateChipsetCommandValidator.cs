using FluentValidation;
using PcBuilderBackend.Application.MasterData.Chipsets.Commands.CreateChipset;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Validators;

public class CreateChipsetCommandValidator : AbstractValidator<CreateChipsetCommand>
{
    public CreateChipsetCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.ManufacturerId)
            .NotEmpty().WithMessage("ManufacturerId is required.");

        RuleFor(x => x.SocketId)
            .NotEmpty().WithMessage("SocketId is required.");
    }
}