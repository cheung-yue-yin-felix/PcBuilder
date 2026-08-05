using FluentValidation;
using PcBuilderBackend.Application.MasterData.Chipsets.Commands.BulkUpdateChipsets;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Validators;

public class BulkUpdateChipsetsCommandValidator: AbstractValidator<BulkUpdateChipsetsCommand>
{
    public BulkUpdateChipsetsCommandValidator()
    {
        RuleFor(x => x.Chipsets).NotEmpty().WithMessage("Chipsets list cannot be empty.");
        
        RuleForEach(x => x.Chipsets).ChildRules(chipset =>
        {
            chipset.RuleFor(x => x.Id).NotEmpty().WithMessage("Chipset ID cannot be empty.");
            chipset.RuleFor(c => c.Name).NotEmpty().WithMessage("Chipset name cannot be empty.");
            chipset.RuleFor(c => c.ManufacturerId).NotEmpty().WithMessage("Manufacturer ID cannot be empty.");
            chipset.RuleFor(c => c.SocketId).NotEmpty().WithMessage("Socket ID cannot be empty.");
        });
    }
}