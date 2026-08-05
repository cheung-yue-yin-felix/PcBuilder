using FluentValidation;
using PcBuilderBackend.Application.MasterData.Manufacturers.Commands.BulkCreateManufacturers;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Validators;

public class BulkCreateManufacturersCommandValidator: AbstractValidator<BulkCreateManufacturersCommand>
{
    public BulkCreateManufacturersCommandValidator()
    {
        RuleForEach(x => x.Names).ChildRules(name =>
        {
            name.RuleFor(x => x)
                .NotEmpty().WithMessage("Manufacturer name cannot be empty.")
                .MaximumLength(100).WithMessage("Manufacturer name cannot exceed 100 characters.");
        });
    }
}