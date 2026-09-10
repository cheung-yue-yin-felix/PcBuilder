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
        RuleForEach(x => x.Cards)
            .SetValidator(new GraphicsCardFieldsValidator<CreateGraphicsCardItem>());
    }
}
