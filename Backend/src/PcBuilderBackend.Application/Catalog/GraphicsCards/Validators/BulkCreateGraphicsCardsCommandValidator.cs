using FluentValidation;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.BulkCreateGraphicsCards;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Validators;

public class BulkCreateGraphicsCardsCommandValidator : AbstractValidator<BulkCreateGraphicsCardsCommand>
{
    public BulkCreateGraphicsCardsCommandValidator()
    {
        RuleFor(x => x.Cards).NotEmpty();
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
        });
    }
}
