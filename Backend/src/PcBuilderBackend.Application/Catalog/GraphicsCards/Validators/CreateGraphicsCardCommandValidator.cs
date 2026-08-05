using FluentValidation;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.CreateGraphicsCard;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Validators;

public class CreateGraphicsCardCommandValidator : AbstractValidator<CreateGraphicsCardCommand>
{
    public CreateGraphicsCardCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ManufacturerId).NotEmpty();
        RuleFor(x => x.GpuId).NotEmpty();
        RuleFor(x => x.VideoMemoryGb).GreaterThan(0);
        RuleFor(x => x.PcieSlotsUsed).GreaterThan(0);
        RuleFor(x => x.PcieGeneration).IsInEnum();
        RuleFor(x => x.LengthMm).GreaterThan(0);
        RuleFor(x => x.WidthMm).GreaterThan(0);
        RuleFor(x => x.HeightMm).GreaterThan(0);
        RuleFor(x => x.PowerConsumptionWatts).GreaterThan(0);

        RuleForEach(x => x.PowerConnectors).ChildRules(connector =>
        {
            connector.RuleFor(c => c.PsuCableType).IsInEnum();
            connector.RuleFor(c => c.ConnectorCount).GreaterThan(0);
        });
    }
}
