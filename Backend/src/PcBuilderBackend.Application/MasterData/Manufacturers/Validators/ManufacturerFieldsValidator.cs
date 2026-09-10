using FluentValidation;
using PcBuilderBackend.Application.MasterData.Manufacturers;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Validators;

public sealed class ManufacturerFieldsValidator<T> : AbstractValidator<T>
    where T : IManufacturerFields
{
    public ManufacturerFieldsValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200);
    }
}
