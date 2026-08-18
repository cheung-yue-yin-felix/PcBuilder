using FluentValidation;
using PcBuilderBackend.Application.Catalog.Motherboards.Commands.BulkUpdateMotherboardM2Slots;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Validators;

public class BulkUpdateMotherboardM2SlotsCommandValidator
    : AbstractValidator<BulkUpdateMotherboardM2SlotsCommand>
{
    public BulkUpdateMotherboardM2SlotsCommandValidator()
    {
        RuleFor(x => x.MotherboardId).NotEmpty();

        RuleFor(x => x.M2Slots)
            .Must(slots => slots
                .Select(s => (s.Key, s.PcieGeneration))
                .Distinct()
                .Count() == slots.Count)
            .WithMessage("Duplicate M.2 slots (same key and PCIe generation) are not allowed.")
            .When(x => x.M2Slots.Count > 0);

        RuleForEach(x => x.M2Slots).ChildRules(slot =>
        {
            slot.RuleFor(x => x.Key)
                .Must(key => key.IsSlotKey())
                .WithMessage("M.2 slot key must be M, B, or E");

            slot.RuleFor(x => x.SupportsSata)
                .Equal(false)
                .When(x => x.Key == M2Key.E)
                .WithMessage("E-key slots cannot support SATA");

            slot.RuleForEach(x => x.FormFactors)
                .IsInEnum().WithMessage("FormFactors is invalid");

            slot.RuleFor(x => x.PcieGeneration)
                .IsInEnum().WithMessage("PcieGeneration is invalid");

            slot.RuleFor(x => x.SlotCount)
                .GreaterThan(0).WithMessage("SlotCount must be greater than 0");
        });
    }
}
