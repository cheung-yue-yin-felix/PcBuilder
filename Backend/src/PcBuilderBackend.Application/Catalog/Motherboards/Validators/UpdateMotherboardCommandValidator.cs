using FluentValidation;
using PcBuilderBackend.Application.Catalog.Motherboards.Commands.UpdateMotherboard;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Validators;

public class UpdateMotherboardCommandValidator : AbstractValidator<UpdateMotherboardCommand>
{
    public UpdateMotherboardCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
        Include(new MotherboardFieldsValidator<UpdateMotherboardCommand>());
    }
}
