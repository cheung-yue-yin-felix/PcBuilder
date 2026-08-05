using FluentValidation;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.UpdateGraphicsCard;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Validators;

public class UpdateGraphicsCardCommandValidator : AbstractValidator<UpdateGraphicsCardCommand>
{
    public UpdateGraphicsCardCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
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
    }
}
