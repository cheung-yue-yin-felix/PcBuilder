using FluentValidation;
using PcBuilderBackend.Application.MasterData.Sockets.Commands.CreateSocket;

namespace PcBuilderBackend.Application.MasterData.Sockets.Validators;

public class CreateSocketCommandValidator: AbstractValidator<CreateSocketCommand>
{
    public CreateSocketCommandValidator()
    {
        RuleFor(x => x.ManufacturerId).NotEmpty().WithMessage("ManufacturerId is required.");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.").MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");
    }
}