using FluentValidation;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Application.MasterData.Chipsets.Commands.CreateChipset;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Validators;

public class CreateChipsetCommandValidator : AbstractValidator<CreateChipsetCommand>
{
    public CreateChipsetCommandValidator(IActiveEntityLookup db)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.ManufacturerId)
            .NotEmpty().WithMessage("ManufacturerId is required.")
            .MustBeActiveManufacturer(db);

        RuleFor(x => x.SocketId)
            .NotEmpty().WithMessage("SocketId is required.")
            .MustBeActiveSocket(db);
    }
}