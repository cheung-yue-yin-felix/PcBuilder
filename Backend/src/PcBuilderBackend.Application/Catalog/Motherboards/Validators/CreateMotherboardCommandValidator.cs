using System.Data;
using FluentValidation;
using PcBuilderBackend.Application.Catalog.Motherboards.Commands.CreateMotherboard;

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
        
        RuleForEach(x => x.M2Slots).ChildRules(slot =>
        {
            slot.RuleFor(x => x.FormFactors)
                .IsInEnum().WithMessage("FormFactors is invalid");

            slot.RuleFor(x => x.PcieGeneration)
                .IsInEnum().WithMessage("PcieGeneration is invalid");

            slot.RuleFor(x => x.SlotCount)
                .GreaterThan(0).WithMessage("SlotCount must be greater than 0");
        });
    }
}
