using FluentValidation;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.CreateGraphicsCard;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Validators;

public class CreateGraphicsCardCommandValidator : AbstractValidator<CreateGraphicsCardCommand>
{
    public CreateGraphicsCardCommandValidator(IActiveEntityLookup db)
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ManufacturerId).NotEmpty().MustBeActiveManufacturer(db);
        RuleFor(x => x.GpuId).NotEmpty().MustBeActiveGpu(db);
        RuleFor(x => x.VideoMemoryGb).GreaterThan(0);
        RuleFor(x => x.PcieSlotsUsed).GreaterThan(0);
        RuleFor(x => x.PcieGeneration).IsInEnum();
        RuleFor(x => x.LengthMm).GreaterThan(0);
        RuleFor(x => x.WidthMm).GreaterThan(0);
        RuleFor(x => x.HeightMm).GreaterThan(0);
        RuleFor(x => x.PowerConsumptionWatts).GreaterThan(0);
        RuleFor(x => x.PowerConnectorType).IsInEnum();
        RuleFor(x => x.PowerConnectorCount).GreaterThan(0);
    }
}
