using FluentValidation;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.DeleteGraphicsCard;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Validators;

public class DeleteGraphicsCardCommandValidator : AbstractValidator<DeleteGraphicsCardCommand>
{
    public DeleteGraphicsCardCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
