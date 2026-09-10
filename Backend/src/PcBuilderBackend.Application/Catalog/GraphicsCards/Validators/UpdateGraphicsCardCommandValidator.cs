using FluentValidation;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.UpdateGraphicsCard;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Validators;

public class UpdateGraphicsCardCommandValidator : AbstractValidator<UpdateGraphicsCardCommand>
{
    public UpdateGraphicsCardCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        Include(new GraphicsCardFieldsValidator<UpdateGraphicsCardCommand>());
    }
}
