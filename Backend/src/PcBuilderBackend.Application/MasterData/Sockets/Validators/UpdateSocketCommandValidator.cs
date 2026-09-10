using FluentValidation;
using PcBuilderBackend.Application.MasterData.Sockets.Commands.UpdateSocket;

namespace PcBuilderBackend.Application.MasterData.Sockets.Validators;

public class UpdateSocketCommandValidator : AbstractValidator<UpdateSocketCommand>
{
    public UpdateSocketCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
        Include(new SocketFieldsValidator<UpdateSocketCommand>());
    }
}
