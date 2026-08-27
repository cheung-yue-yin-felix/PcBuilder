using FluentValidation;
using PcBuilderBackend.Application.MasterData.Sockets.Commands.BulkUpdateSockets;

namespace PcBuilderBackend.Application.MasterData.Sockets.Validators;

public class BulkUpdateSocketsCommandValidator: AbstractValidator<BulkUpdateSocketsCommand>
{
    public BulkUpdateSocketsCommandValidator()
    {
        RuleFor(x => x.Sockets).NotEmpty().WithMessage("Sockets list cannot be empty.");
        RuleForEach(x => x.Sockets).ChildRules(sockets =>
        {
            sockets.RuleFor(socket => socket.Id).NotEmpty().WithMessage("Socket Id cannot be empty.");
            sockets.RuleFor(socket => socket.ManufacturerId).NotEmpty().WithMessage("Manufacturer Id cannot be empty.");
            sockets.RuleFor(socket => socket.Name).NotEmpty().WithMessage("Socket Name cannot be empty.").MaximumLength(200).WithMessage("Socket Name cannot exceed 200 characters.");
        });
    }
}