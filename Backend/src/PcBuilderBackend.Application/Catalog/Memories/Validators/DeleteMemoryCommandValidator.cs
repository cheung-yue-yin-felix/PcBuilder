using FluentValidation;
using PcBuilderBackend.Application.Catalog.Memories.Commands.DeleteMemory;

namespace PcBuilderBackend.Application.Catalog.Memories.Validators;

public class DeleteMemoryCommandValidator : AbstractValidator<DeleteMemoryCommand>
{
    public DeleteMemoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
    }
}
