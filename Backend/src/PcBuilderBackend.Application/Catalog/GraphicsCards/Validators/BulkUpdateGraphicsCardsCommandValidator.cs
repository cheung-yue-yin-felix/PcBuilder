using FluentValidation;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.BulkUpdateGraphicsCards;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Validators;

public class BulkUpdateGraphicsCardsCommandValidator : AbstractValidator<BulkUpdateGraphicsCardsCommand>
{
    public BulkUpdateGraphicsCardsCommandValidator()
    {
        RuleFor(x => x.Cards).NotEmpty();
        RuleForEach(x => x.Cards).SetValidator(new UpdateGraphicsCardCommandValidator());
    }
}
