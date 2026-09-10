using FluentValidation;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Commands.UpdateWiredNetworkAdapter;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Validators;

public class UpdateWiredNetworkAdapterCommandValidator : AbstractValidator<UpdateWiredNetworkAdapterCommand>
{
    public UpdateWiredNetworkAdapterCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        Include(new WiredNetworkAdapterFieldsValidator<UpdateWiredNetworkAdapterCommand>());
    }
}
