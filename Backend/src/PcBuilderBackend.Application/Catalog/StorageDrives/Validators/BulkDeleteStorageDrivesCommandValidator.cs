using FluentValidation;
using PcBuilderBackend.Application.Catalog.StorageDrives.Commands.BulkDeleteStorageDrives;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Validators;

public class BulkDeleteStorageDrivesCommandValidator : AbstractValidator<BulkDeleteStorageDrivesCommand>
{
    public BulkDeleteStorageDrivesCommandValidator()
    {
        RuleFor(x => x.Ids).NotEmpty();
    }
}
