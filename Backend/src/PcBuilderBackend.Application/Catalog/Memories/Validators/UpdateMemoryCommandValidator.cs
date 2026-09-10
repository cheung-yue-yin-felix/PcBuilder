using FluentValidation;
using PcBuilderBackend.Application.Catalog.Memories.Commands.UpdateMemory;

namespace PcBuilderBackend.Application.Catalog.Memories.Validators;

public class UpdateMemoryCommandValidator : AbstractValidator<UpdateMemoryCommand>
{
    public UpdateMemoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
        Include(new MemoryFieldsValidator<UpdateMemoryCommand>());
    }
}
