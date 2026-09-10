using FluentValidation;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Commands.BulkCreateWirelessNetworkAdapters;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Validators;

public class BulkCreateWirelessNetworkAdaptersCommandValidator
    : AbstractValidator<BulkCreateWirelessNetworkAdaptersCommand>
{
    public BulkCreateWirelessNetworkAdaptersCommandValidator(IActiveEntityLookup db)
    {
        RuleFor(x => x.Adapters).NotEmpty();
        RuleFor(x => x.Adapters.Select(a => a.ManufacturerId))
            .MustAllBeActiveManufacturers(db)
            .When(x => x.Adapters is { Count: > 0 });
        RuleForEach(x => x.Adapters)
            .SetValidator(new WirelessNetworkAdapterFieldsValidator<CreateWirelessNetworkAdapterItem>());
    }
}
