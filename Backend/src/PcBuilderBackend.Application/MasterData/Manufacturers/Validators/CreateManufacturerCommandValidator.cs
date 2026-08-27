using FluentValidation;
using PcBuilderBackend.Application.MasterData.Manufacturers.Commands.CreateManufacturer;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Validators;

public class CreateManufacturerCommandValidator: AbstractValidator<CreateManufacturerCommand>
{
    public CreateManufacturerCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200);
    }
}
