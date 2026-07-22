using FluentValidation;
using PcBuilderBackend.Application.Catalog.Manufacturers.Commands.UpdateManufacturer;

namespace PcBuilderBackend.Application.Catalog.Manufacturers.Validators;

public class UpdateManufacturerCommandValidator: AbstractValidator<UpdateManufacturerCommand>
{
    public  UpdateManufacturerCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required").MaximumLength(200);
    }
}