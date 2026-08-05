using FluentValidation;
using PcBuilderBackend.Application.MasterData.Sockets.Commands.DeleteSocket;

namespace PcBuilderBackend.Application.MasterData.Sockets.Validators;

public class DeleteSocketCommandValidator: AbstractValidator<DeleteSocketCommand>
{
    public DeleteSocketCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}