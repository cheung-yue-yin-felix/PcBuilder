using FluentValidation;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Commands.BulkDeleteWiredNetworkAdapters;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Validators;

public class BulkDeleteWiredNetworkAdaptersCommandValidator : AbstractValidator<BulkDeleteWiredNetworkAdaptersCommand>
{
    public BulkDeleteWiredNetworkAdaptersCommandValidator()
    {
        RuleFor(x => x.Ids).NotEmpty();
    }
}
