using FluentValidation;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.CreateGraphicsCard;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Validators;

public class CreateGraphicsCardCommandValidator : AbstractValidator<CreateGraphicsCardCommand>
{
    public CreateGraphicsCardCommandValidator(IActiveEntityLookup db)
    {
        Include(new GraphicsCardFieldsValidator<CreateGraphicsCardCommand>());
        RuleFor(x => x.ManufacturerId).MustBeActiveManufacturer(db);
        RuleFor(x => x.GpuId).MustBeActiveGpu(db);
    }
}
