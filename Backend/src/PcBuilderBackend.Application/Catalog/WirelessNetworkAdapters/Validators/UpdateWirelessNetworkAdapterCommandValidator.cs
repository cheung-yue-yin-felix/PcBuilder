using FluentValidation;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Commands.UpdateWirelessNetworkAdapter;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Validators;

public class UpdateWirelessNetworkAdapterCommandValidator : AbstractValidator<UpdateWirelessNetworkAdapterCommand>
{
    public UpdateWirelessNetworkAdapterCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        Include(new WirelessNetworkAdapterFieldsValidator<UpdateWirelessNetworkAdapterCommand>());
    }
}
