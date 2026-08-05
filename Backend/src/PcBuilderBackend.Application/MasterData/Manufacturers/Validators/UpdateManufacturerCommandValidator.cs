using FluentValidation;
using PcBuilderBackend.Application.MasterData.Manufacturers.Commands.UpdateManufacturer;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Validators;

public class UpdateManufacturerCommandValidator: AbstractValidator<UpdateManufacturerCommand>
{
    public  UpdateManufacturerCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required").MaximumLength(200);
    }
}
