using System.Data;
using FluentValidation;
using PcBuilderBackend.Application.MasterData.Sockets.Commands.UpdateSocket;

namespace PcBuilderBackend.Application.MasterData.Sockets.Validators;

public class UpdateSocketCommandValidator: AbstractValidator<UpdateSocketCommand>
{
    public UpdateSocketCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
        RuleFor(x => x.ManufacturerId).NotEmpty().WithMessage("ManufacturerId is required.");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.").MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");
    }
}