using FluentValidation;
using PcBuilderBackend.Application.Catalog.Psus.Dto;
using PcBuilderBackend.Application.Catalog.Psus;

namespace PcBuilderBackend.Application.Catalog.Psus.Validators;

public sealed class PsuFieldsValidator<T> : AbstractValidator<T>
    where T : IPsuFields
{
    public PsuFieldsValidator()
    {
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

public sealed class PsuCableRulesValidator<T> : AbstractValidator<T>
    where T : IPsuCables
{
    public PsuCableRulesValidator()
    {
        RuleFor(x => x.Cables)
            .Must(BeUniqueCableTypes)
            .WithMessage("Duplicate PSU cable types are not allowed.")
            .When(x => x.Cables.Count > 0);

        RuleForEach(x => x.Cables).ChildRules(cable =>
        {
            cable.RuleFor(c => c.Type).IsInEnum();
            cable.RuleFor(c => c.CablesCount).GreaterThan(0);
            cable.RuleFor(c => c.ConnectorsCount).GreaterThan(0);
        });
    }

    private static bool BeUniqueCableTypes(List<PsuCableDto> cables) =>
        cables.GroupBy(x => x.Type).All(g => g.Count() == 1);
}
