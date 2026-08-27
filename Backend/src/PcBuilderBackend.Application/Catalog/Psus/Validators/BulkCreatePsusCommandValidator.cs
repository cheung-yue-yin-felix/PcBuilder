using FluentValidation;
using PcBuilderBackend.Application.Catalog.Psus.Commands.BulkCreatePsus;
using PcBuilderBackend.Application.Catalog.Psus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;

namespace PcBuilderBackend.Application.Catalog.Psus.Validators;

public class BulkCreatePsusCommandValidator : AbstractValidator<BulkCreatePsusCommand>
{
    public BulkCreatePsusCommandValidator(IActiveEntityLookup db)
    {
        RuleFor(x => x.Psus).NotEmpty();
        RuleFor(x => x.Psus.Select(p => p.ManufacturerId))
            .MustAllBeActiveManufacturers(db)
            .When(x => x.Psus is { Count: > 0 });

        RuleForEach(x => x.Psus).ChildRules(item =>
        {
            item.RuleFor(p => p.Name).NotEmpty().MaximumLength(200);
            item.RuleFor(p => p.ManufacturerId).NotEmpty();
            item.RuleFor(p => p.Wattage).GreaterThan(0);
            item.RuleFor(p => p.Modularity).IsInEnum();
            item.RuleFor(p => p.FormFactor).IsInEnum();
            item.RuleFor(p => p.LengthMm).GreaterThan(0);
            item.RuleFor(p => p.WidthMm).GreaterThan(0);
            item.RuleFor(p => p.HeightMm).GreaterThan(0);

            item.RuleFor(p => p.Cables)
                .Must(BeUniqueCableTypes)
                .WithMessage("Duplicate PSU cable types are not allowed.")
                .When(p => p.Cables.Count > 0);

            item.RuleForEach(p => p.Cables).ChildRules(cable =>
            {
                cable.RuleFor(c => c.Type).IsInEnum();
                cable.RuleFor(c => c.CablesCount).GreaterThan(0);
                cable.RuleFor(c => c.ConnectorsCount).GreaterThan(0);
            });
        });
    }

    private static bool BeUniqueCableTypes(List<PsuCableDto> cables) =>
        cables.GroupBy(x => x.Type).All(g => g.Count() == 1);
}
