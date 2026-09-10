using FluentValidation;
using PcBuilderBackend.Application.MasterData.Sockets;

namespace PcBuilderBackend.Application.MasterData.Sockets.Validators;

public sealed class SocketFieldsValidator<T> : AbstractValidator<T>
    where T : ISocketFields
{
    public SocketFieldsValidator()
    {
        RuleFor(x => x.ManufacturerId)
            .NotEmpty().WithMessage("ManufacturerId is required.");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");
    }
}
