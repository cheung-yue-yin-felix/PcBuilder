using FluentValidation;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Application.MasterData.Chipsets.Commands.CreateChipset;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Validators;

public class CreateChipsetCommandValidator : AbstractValidator<CreateChipsetCommand>
{
    public CreateChipsetCommandValidator(IActiveEntityLookup db)
    {
        Include(new ChipsetFieldsValidator<CreateChipsetCommand>());
        RuleFor(x => x.ManufacturerId).MustBeActiveManufacturer(db);
        RuleFor(x => x.SocketId).MustBeActiveSocket(db);
    }
}
