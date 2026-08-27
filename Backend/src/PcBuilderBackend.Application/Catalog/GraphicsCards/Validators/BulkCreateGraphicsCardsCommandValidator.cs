using FluentValidation;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.BulkCreateGraphicsCards;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Validators;

public class BulkCreateGraphicsCardsCommandValidator : AbstractValidator<BulkCreateGraphicsCardsCommand>
{
    public BulkCreateGraphicsCardsCommandValidator(IActiveEntityLookup db)
    {
        RuleFor(x => x.Cards).NotEmpty();
        RuleFor(x => x.Cards.Select(c => c.ManufacturerId))
            .MustAllBeActiveManufacturers(db)
            .When(x => x.Cards is { Count: > 0 });
        RuleFor(x => x.Cards.Select(c => c.GpuId))
            .MustAllBeActiveGpus(db)
            .When(x => x.Cards is { Count: > 0 });
        RuleForEach(x => x.Cards).ChildRules(card =>
        {
            card.RuleFor(c => c.Name).NotEmpty().MaximumLength(200);
            card.RuleFor(c => c.ManufacturerId).NotEmpty();
            card.RuleFor(c => c.GpuId).NotEmpty();
            card.RuleFor(c => c.VideoMemoryGb).GreaterThan(0);
            card.RuleFor(c => c.PcieSlotsUsed).GreaterThan(0);
            card.RuleFor(c => c.PcieGeneration).IsInEnum();
            card.RuleFor(c => c.LengthMm).GreaterThan(0);
            card.RuleFor(c => c.WidthMm).GreaterThan(0);
            card.RuleFor(c => c.HeightMm).GreaterThan(0);
            card.RuleFor(c => c.PowerConsumptionWatts).GreaterThan(0);
            card.RuleFor(c => c.PowerConnectorType).IsInEnum();
            card.RuleFor(c => c.PowerConnectorCount).GreaterThan(0);
        });
    }
}
