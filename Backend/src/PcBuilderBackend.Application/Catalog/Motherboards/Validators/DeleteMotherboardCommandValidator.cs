using FluentValidation;
using PcBuilderBackend.Application.Catalog.Motherboards.Commands.DeleteMotherboard;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Validators;

public class DeleteMotherboardCommandValidator : AbstractValidator<DeleteMotherboardCommand>
{
    public DeleteMotherboardCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
    }
}
