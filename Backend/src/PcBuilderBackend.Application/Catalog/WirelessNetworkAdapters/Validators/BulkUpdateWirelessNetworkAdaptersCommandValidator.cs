using FluentValidation;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Commands.BulkUpdateWirelessNetworkAdapters;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Validators;

public class BulkUpdateWirelessNetworkAdaptersCommandValidator
    : AbstractValidator<BulkUpdateWirelessNetworkAdaptersCommand>
{
    public BulkUpdateWirelessNetworkAdaptersCommandValidator()
    {
        RuleFor(x => x.Adapters).NotEmpty();
        RuleForEach(x => x.Adapters).SetValidator(new UpdateWirelessNetworkAdapterCommandValidator());
    }
}
