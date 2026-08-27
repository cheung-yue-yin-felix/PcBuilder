using FluentValidation;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Application.MasterData.Chipsets.Commands.BulkCreateChipsets;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Validators;

public class BulkCreateChipsetsCommandValidator: AbstractValidator<BulkCreateChipsetsCommand>
{
    public BulkCreateChipsetsCommandValidator(IActiveEntityLookup db)
    {
        RuleFor(x => x.Chipsets).NotEmpty().WithMessage("Chipsets list cannot be empty.");

        RuleFor(x => x.Chipsets.Select(c => c.ManufacturerId))
            .MustAllBeActiveManufacturers(db)
            .When(x => x.Chipsets is { Count: > 0 });
        RuleFor(x => x.Chipsets.Select(c => c.SocketId))
            .MustAllBeActiveSockets(db)
            .When(x => x.Chipsets is { Count: > 0 });
        
        RuleForEach(x => x.Chipsets).ChildRules(chipset =>
        {
            chipset.RuleFor(c => c.Name).NotEmpty().WithMessage("Chipset name cannot be empty.");
            chipset.RuleFor(c => c.ManufacturerId).NotEmpty().WithMessage("Manufacturer ID cannot be empty.");
            chipset.RuleFor(c => c.SocketId).NotEmpty().WithMessage("Socket ID cannot be empty.");
        });
    }
}