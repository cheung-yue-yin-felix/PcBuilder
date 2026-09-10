using FluentValidation;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Commands.CreateWiredNetworkAdapter;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Validators;

public class CreateWiredNetworkAdapterCommandValidator : AbstractValidator<CreateWiredNetworkAdapterCommand>
{
    public CreateWiredNetworkAdapterCommandValidator(IActiveEntityLookup db)
    {
        Include(new WiredNetworkAdapterFieldsValidator<CreateWiredNetworkAdapterCommand>());
        RuleFor(x => x.ManufacturerId).MustBeActiveManufacturer(db);
    }
}
