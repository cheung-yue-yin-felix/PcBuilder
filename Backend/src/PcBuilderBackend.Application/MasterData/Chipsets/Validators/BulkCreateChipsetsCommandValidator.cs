using FluentValidation;
using PcBuilderBackend.Application.MasterData.Chipsets.Commands.BulkCreateChipsets;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Validators;

public class BulkCreateChipsetsCommandValidator: AbstractValidator<BulkCreateChipsetsCommand>
{
    public BulkCreateChipsetsCommandValidator()
    {
        RuleFor(x => x.Chipsets).NotEmpty().WithMessage("Chipsets list cannot be empty.");
        
        RuleForEach(x => x.Chipsets).ChildRules(chipset =>
        {
            chipset.RuleFor(c => c.Name).NotEmpty().WithMessage("Chipset name cannot be empty.");
            chipset.RuleFor(c => c.ManufacturerId).NotEmpty().WithMessage("Manufacturer ID cannot be empty.");
            chipset.RuleFor(c => c.SocketId).NotEmpty().WithMessage("Socket ID cannot be empty.");
        });
    }
}