using FluentValidation;
using PcBuilderBackend.Application.MasterData.Manufacturers.Commands.DeleteManufacturer;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Validators;

public class DeleteManufacturerCommandValidator: AbstractValidator<DeleteManufacturerCommand>
{
    public DeleteManufacturerCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}
