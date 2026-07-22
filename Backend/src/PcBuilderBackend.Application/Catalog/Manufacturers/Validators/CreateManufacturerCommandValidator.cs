using FluentValidation;
using PcBuilderBackend.Application.Catalog.Manufacturers.Commands.CreateManufacturer;

namespace PcBuilderBackend.Application.Catalog.Manufacturers.Validators;

public class CreateManufacturerCommandValidator: AbstractValidator<CreateManufacturerCommand>
{
    public CreateManufacturerCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200);
    }
}