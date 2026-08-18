using FluentValidation;
using PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkCreateChassis;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;

namespace PcBuilderBackend.Application.Catalog.Chassis.Validators;

public class BulkCreateChassisCommandValidator : AbstractValidator<BulkCreateChassisCommand>
{
    public BulkCreateChassisCommandValidator()
    {
        RuleFor(x => x.Items).NotEmpty();

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(c => c.Name).NotEmpty().MaximumLength(200);
            item.RuleFor(c => c.ManufacturerId).NotEmpty();
            item.RuleFor(c => c.LengthMm).GreaterThan(0);
            item.RuleFor(c => c.WidthMm).GreaterThan(0);
            item.RuleFor(c => c.HeightMm).GreaterThan(0);
            item.RuleFor(c => c.MotherboardMaxWidthMm).GreaterThan(0);
            item.RuleFor(c => c.MotherboardMaxHeightMm).GreaterThan(0);
            item.RuleFor(c => c.MaxCpuCoolerHeightMm).GreaterThan(0);
            item.RuleFor(c => c.MaxGraphicsCardLengthMm).GreaterThan(0);
            item.RuleFor(c => c.MaxPsuLengthMm).GreaterThan(0);

            item.RuleForEach(c => c.DriveBays).ChildRules(bay =>
            {
                bay.RuleFor(b => b.FormFactor).IsInEnum();
                bay.RuleFor(b => b.SlotCount).GreaterThan(0);
            });

            item.RuleForEach(c => c.FanMounts).ChildRules(mount =>
            {
                mount.RuleFor(m => m.Location).IsInEnum();
                mount.RuleForEach(m => m.Options).ChildRules(option =>
                {
                    option.RuleFor(o => o.Diameter).IsInEnum();
                    option.RuleFor(o => o.SlotCount).GreaterThan(0);
                });
            });

            item.RuleForEach(c => c.PcieSlots).ChildRules(slot =>
            {
                slot.RuleFor(s => s.Orientation).IsInEnum();
                slot.RuleFor(s => s.SlotCount).GreaterThan(0);
            });

            item.RuleForEach(c => c.Radiators).ChildRules(radiator =>
            {
                radiator.RuleFor(r => r.Length).IsInEnum();
                radiator.RuleFor(r => r.Location).IsInEnum();
                radiator.RuleFor(r => r.RadiatorCount).GreaterThan(0);
            });

            item.RuleForEach(c => c.MbFormFactors).IsInEnum();
            item.RuleForEach(c => c.PsuFormFactors).IsInEnum();
        });
    }
}
