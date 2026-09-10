using FluentValidation;
using PcBuilderBackend.Application.MasterData.Chipsets;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Validators;

public sealed class ChipsetFieldsValidator<T> : AbstractValidator<T>
    where T : IChipsetFields
{
    public ChipsetFieldsValidator()
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
