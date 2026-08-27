using FluentValidation;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Commands.DeleteWiredNetworkAdapter;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Validators;

public class DeleteWiredNetworkAdapterCommandValidator : AbstractValidator<DeleteWiredNetworkAdapterCommand>
{
    public DeleteWiredNetworkAdapterCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
