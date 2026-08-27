using FluentValidation;
using PcBuilderBackend.Application.Catalog.Psus.Commands.BulkUpdatePsuCables;
using PcBuilderBackend.Application.Catalog.Psus.Dto;

namespace PcBuilderBackend.Application.Catalog.Psus.Validators;

public class BulkUpdatePsuCablesCommandValidator : AbstractValidator<BulkUpdatePsuCablesCommand>
{
    public BulkUpdatePsuCablesCommandValidator()
    {
        RuleFor(x => x.PsuId).NotEmpty();

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
