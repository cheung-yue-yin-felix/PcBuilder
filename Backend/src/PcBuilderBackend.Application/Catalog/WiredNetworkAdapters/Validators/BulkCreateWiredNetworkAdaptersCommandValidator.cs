using FluentValidation;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Commands.BulkCreateWiredNetworkAdapters;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Validators;

public class BulkCreateWiredNetworkAdaptersCommandValidator : AbstractValidator<BulkCreateWiredNetworkAdaptersCommand>
{
    public BulkCreateWiredNetworkAdaptersCommandValidator(IActiveEntityLookup db)
    {
        RuleFor(x => x.Adapters).NotEmpty();
        RuleFor(x => x.Adapters.Select(a => a.ManufacturerId))
            .MustAllBeActiveManufacturers(db)
            .When(x => x.Adapters is { Count: > 0 });
        RuleForEach(x => x.Adapters)
            .SetValidator(new WiredNetworkAdapterFieldsValidator<CreateWiredNetworkAdapterItem>());
    }
}
