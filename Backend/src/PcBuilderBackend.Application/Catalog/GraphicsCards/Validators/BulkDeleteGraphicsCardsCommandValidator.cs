using FluentValidation;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.BulkDeleteGraphicsCards;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Validators;

public class BulkDeleteGraphicsCardsCommandValidator : AbstractValidator<BulkDeleteGraphicsCardsCommand>
{
    public BulkDeleteGraphicsCardsCommandValidator()
    {
        RuleFor(x => x.Ids).NotEmpty();
    }
}
