using FluentValidation;
using PcBuilderBackend.Application.Catalog.Manufacturers.Commands.DeleteManufacturer;

namespace PcBuilderBackend.Application.Catalog.Manufacturers.Validators;

public class DeleteManufacturerCommandValidator: AbstractValidator<DeleteManufacturerCommand>
{
    public DeleteManufacturerCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}