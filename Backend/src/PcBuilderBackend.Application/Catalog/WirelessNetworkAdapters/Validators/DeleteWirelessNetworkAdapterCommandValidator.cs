using FluentValidation;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Commands.DeleteWirelessNetworkAdapter;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Validators;

public class DeleteWirelessNetworkAdapterCommandValidator : AbstractValidator<DeleteWirelessNetworkAdapterCommand>
{
    public DeleteWirelessNetworkAdapterCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
