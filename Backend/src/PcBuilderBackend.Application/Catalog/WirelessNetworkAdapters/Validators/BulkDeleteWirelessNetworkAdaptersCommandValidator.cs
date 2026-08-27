using FluentValidation;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Commands.BulkDeleteWirelessNetworkAdapters;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Validators;

public class BulkDeleteWirelessNetworkAdaptersCommandValidator
    : AbstractValidator<BulkDeleteWirelessNetworkAdaptersCommand>
{
    public BulkDeleteWirelessNetworkAdaptersCommandValidator()
    {
        RuleFor(x => x.Ids).NotEmpty();
    }
}
