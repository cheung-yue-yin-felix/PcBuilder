using FluentValidation;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Commands.CreateWirelessNetworkAdapter;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Validators;

public class CreateWirelessNetworkAdapterCommandValidator : AbstractValidator<CreateWirelessNetworkAdapterCommand>
{
    public CreateWirelessNetworkAdapterCommandValidator(IActiveEntityLookup db)
    {
        Include(new WirelessNetworkAdapterFieldsValidator<CreateWirelessNetworkAdapterCommand>());
        RuleFor(x => x.ManufacturerId).MustBeActiveManufacturer(db);
    }
}
