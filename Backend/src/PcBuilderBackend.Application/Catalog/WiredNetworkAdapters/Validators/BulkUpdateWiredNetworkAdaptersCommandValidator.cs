using FluentValidation;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Commands.BulkUpdateWiredNetworkAdapters;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Validators;

public class BulkUpdateWiredNetworkAdaptersCommandValidator : AbstractValidator<BulkUpdateWiredNetworkAdaptersCommand>
{
    public BulkUpdateWiredNetworkAdaptersCommandValidator()
    {
        RuleFor(x => x.Adapters).NotEmpty();
        RuleForEach(x => x.Adapters).SetValidator(new UpdateWiredNetworkAdapterCommandValidator());
    }
}
