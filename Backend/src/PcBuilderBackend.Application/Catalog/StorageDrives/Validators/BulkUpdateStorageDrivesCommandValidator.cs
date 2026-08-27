using FluentValidation;
using PcBuilderBackend.Application.Catalog.StorageDrives.Commands.BulkUpdateStorageDrives;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Validators;

public class BulkUpdateStorageDrivesCommandValidator : AbstractValidator<BulkUpdateStorageDrivesCommand>
{
    public BulkUpdateStorageDrivesCommandValidator()
    {
        RuleFor(x => x.Drives).NotEmpty();
        RuleForEach(x => x.Drives).SetValidator(new UpdateStorageDriveCommandValidator());
    }
}
