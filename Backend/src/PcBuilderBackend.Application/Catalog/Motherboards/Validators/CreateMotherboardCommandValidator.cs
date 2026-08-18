using FluentValidation;
using PcBuilderBackend.Application.Catalog.Motherboards.Commands.CreateMotherboard;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Validators;

public class CreateMotherboardCommandValidator : AbstractValidator<CreateMotherboardCommand>
{
    public CreateMotherboardCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200);

        RuleFor(x => x.ManufacturerId)
            .NotEmpty().WithMessage("ManufacturerId is required");

        RuleFor(x => x.SocketId)
            .NotEmpty().WithMessage("SocketId is required");

        RuleFor(x => x.ChipsetId)
            .NotEmpty().WithMessage("ChipsetId is required");

        RuleFor(x => x.RamSlots)
            .GreaterThan(0).WithMessage("RamSlots must be greater than 0");

        RuleFor(x => x.MaxMemoryGb)
            .GreaterThan(0).WithMessage("MaxMemoryGb must be greater than 0");

        RuleFor(x => x.MaxDimmSizeGb)
            .GreaterThan(0).WithMessage("MaxDimmSizeGb must be greater than 0");

        RuleFor(x => x.SataPorts)
            .GreaterThanOrEqualTo(0).WithMessage("SataPorts cannot be negative");

        RuleFor(x => x.FanConnectors)
            .GreaterThanOrEqualTo(0).WithMessage("FanConnectors cannot be negative");

        RuleFor(x => x.EpsConnectors)
            .GreaterThanOrEqualTo(0).WithMessage("EpsConnectors cannot be negative");

        RuleFor(x => x.WidthMm)
            .GreaterThan(0).WithMessage("WidthMm must be greater than 0");

        RuleFor(x => x.HeightMm)
            .GreaterThan(0).WithMessage("HeightMm must be greater than 0");

        RuleFor(x => x.DdrGeneration)
            .IsInEnum().WithMessage("DdrGeneration is invalid");

        RuleFor(x => x.RamFormFactor)
            .IsInEnum().WithMessage("RamFormFactor is invalid");

        RuleFor(x => x.FormFactor)
            .IsInEnum().WithMessage("FormFactor is invalid");
        
        RuleFor(x => x.PcieSlots)
            .NotEmpty().WithMessage("PcieSlots is required");

        RuleFor(x => x.M2Slots)
            .NotEmpty().WithMessage("M2Slots is required");

        RuleFor(x => x.UsbPorts)
            .NotEmpty().WithMessage("UsbPorts is required");
        
        RuleForEach(x => x.PcieSlots).ChildRules(slot =>
        {
            slot.RuleFor(x => x.Generation)
                .IsInEnum().WithMessage("Generation is invalid");

            slot.RuleFor(x => x.SlotType)
                .IsInEnum().WithMessage("SlotType is invalid");

            slot.RuleFor(x => x.SlotLanes)
                .IsInEnum().WithMessage("SlotLane is invalid");
            
            slot.RuleFor(x => x.SlotCount)
                .GreaterThan(0).WithMessage("SlotCount must be greater than 0");
        });
        
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

        RuleFor(x => x.UsbPorts)
            .Must(ports => ports
                .Select(p => (p.UsbType, p.UsbVersion))
                .Distinct()
                .Count() == ports.Count)
            .WithMessage("Duplicate USB ports (same type and version) are not allowed.")
            .When(x => x.UsbPorts.Count > 0);

        RuleForEach(x => x.UsbPorts).ChildRules(port =>
        {
            port.RuleFor(x => x.UsbVersion).IsInEnum().WithMessage("UsbVersion is invalid");
            port.RuleFor(x => x.UsbType).IsInEnum().WithMessage("UsbType is invalid");
            port.RuleFor(x => x.PortCount).GreaterThan(0).WithMessage("PortCount must be greater than 0");
        });
    }
}
