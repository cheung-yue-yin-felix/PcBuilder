using FluentValidation;
using PcBuilderBackend.Application.Catalog.Motherboards.Commands.UpdateMotherboard;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Validators;

public class UpdateMotherboardCommandValidator : AbstractValidator<UpdateMotherboardCommand>
{
    public UpdateMotherboardCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");

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
    }
}
