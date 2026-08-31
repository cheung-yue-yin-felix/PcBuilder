using FluentValidation;
using PcBuilderBackend.Application.MasterData.Manufacturers.Commands.BulkDeleteManufacturers;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Validators;

public class BulkDeleteManufacturersCommandValidator: AbstractValidator<BulkDeleteManufacturersCommand>
{
    public BulkDeleteManufacturersCommandValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithMessage("Ids list cannot be empty.");
        RuleForEach(x => x.Ids).NotEmpty().WithMessage("Id is required");
    }
}
