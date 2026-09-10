using FluentValidation;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Application.MasterData.Sockets.Commands.CreateSocket;

namespace PcBuilderBackend.Application.MasterData.Sockets.Validators;

public class CreateSocketCommandValidator : AbstractValidator<CreateSocketCommand>
{
    public CreateSocketCommandValidator(IActiveEntityLookup db)
    {
        Include(new SocketFieldsValidator<CreateSocketCommand>());
        RuleFor(x => x.ManufacturerId).MustBeActiveManufacturer(db);
    }
}
