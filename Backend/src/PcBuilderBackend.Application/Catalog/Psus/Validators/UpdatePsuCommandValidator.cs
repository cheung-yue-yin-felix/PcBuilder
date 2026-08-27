using FluentValidation;
using PcBuilderBackend.Application.Catalog.Psus.Commands.UpdatePsu;

namespace PcBuilderBackend.Application.Catalog.Psus.Validators;

public class UpdatePsuCommandValidator : AbstractValidator<UpdatePsuCommand>
{
    public UpdatePsuCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ManufacturerId).NotEmpty();
        RuleFor(x => x.Wattage).GreaterThan(0);
        RuleFor(x => x.Modularity).IsInEnum();
        RuleFor(x => x.FormFactor).IsInEnum();
        RuleFor(x => x.LengthMm).GreaterThan(0);
        RuleFor(x => x.WidthMm).GreaterThan(0);
        RuleFor(x => x.HeightMm).GreaterThan(0);
    }
}
