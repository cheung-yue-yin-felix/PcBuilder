using FluentValidation;
using PcBuilderBackend.Application.Catalog.Memories.Commands.CreateMemory;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;

namespace PcBuilderBackend.Application.Catalog.Memories.Validators;

public class CreateMemoryCommandValidator : AbstractValidator<CreateMemoryCommand>
{
    public CreateMemoryCommandValidator(IActiveEntityLookup db)
    {
        Include(new MemoryFieldsValidator<CreateMemoryCommand>());
        RuleFor(x => x.ManufacturerId).MustBeActiveManufacturer(db);
    }
}
