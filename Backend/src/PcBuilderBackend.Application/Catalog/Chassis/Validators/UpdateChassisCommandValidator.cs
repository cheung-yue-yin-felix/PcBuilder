using FluentValidation;
using PcBuilderBackend.Application.Catalog.Chassis.Commands.UpdateChassis;

namespace PcBuilderBackend.Application.Catalog.Chassis.Validators;

public class UpdateChassisCommandValidator : AbstractValidator<UpdateChassisCommand>
{
    public UpdateChassisCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ManufacturerId).NotEmpty();
        RuleFor(x => x.LengthMm).GreaterThan(0);
        RuleFor(x => x.WidthMm).GreaterThan(0);
        RuleFor(x => x.HeightMm).GreaterThan(0);
        RuleFor(x => x.MotherboardMaxWidthMm).GreaterThan(0);
        RuleFor(x => x.MotherboardMaxHeightMm).GreaterThan(0);
        RuleFor(x => x.MaxCpuCoolerHeightMm).GreaterThan(0);
        RuleFor(x => x.MaxGraphicsCardLengthMm).GreaterThan(0);
        RuleFor(x => x.MaxPsuLengthMm).GreaterThan(0);
    }
}
