using FluentValidation;
using PcBuilderBackend.Application.MasterData.Manufacturers.Commands.BulkDeleteManufacturers;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Validators;

public class BulkDeleteManufacturersCommandValidator: AbstractValidator<BulkDeleteManufacturersCommand>
{
    public BulkDeleteManufacturersCommandValidator()
    {
        RuleForEach(x => x.ManufacturerIds).ChildRules(manufacturer =>
        {
            manufacturer.RuleFor(x => x)
                .NotEmpty().WithMessage("ManufacturerId is required");
        });
    }
}
