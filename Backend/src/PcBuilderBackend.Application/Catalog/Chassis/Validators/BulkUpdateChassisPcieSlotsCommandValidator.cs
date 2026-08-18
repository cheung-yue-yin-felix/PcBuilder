using FluentValidation;
using PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkUpdateChassisPcieSlots;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;

namespace PcBuilderBackend.Application.Catalog.Chassis.Validators;

public class BulkUpdateChassisPcieSlotsCommandValidator
    : AbstractValidator<BulkUpdateChassisPcieSlotsCommand>
{
    public BulkUpdateChassisPcieSlotsCommandValidator()
    {
        RuleFor(x => x.ChassisId).NotEmpty();

        RuleFor(x => x.PcieSlots)
            .Must(BeUniqueByKey)
            .WithMessage("Duplicate PCIe slots (same LowProfileSlots and Orientation) are not allowed.")
            .When(x => x.PcieSlots.Count > 0);

        RuleForEach(x => x.PcieSlots).ChildRules(slot =>
        {
            slot.RuleFor(s => s.Orientation).IsInEnum();
            slot.RuleFor(s => s.SlotCount).GreaterThan(0);
        });
    }

    private static bool BeUniqueByKey(List<ChassisPcieSlotDto> slots) =>
        slots.GroupBy(x => (x.LowProfileSlots, x.Orientation)).All(g => g.Count() == 1);
}
