using FluentValidation;
using PcBuilderBackend.Application.Catalog.StorageDrives.Commands.BulkCreateStorageDrives;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Validators;

public class BulkCreateStorageDrivesCommandValidator : AbstractValidator<BulkCreateStorageDrivesCommand>
{
    public BulkCreateStorageDrivesCommandValidator(IActiveEntityLookup db)
    {
        RuleFor(x => x.Drives).NotEmpty();
        RuleFor(x => x.Drives.Select(d => d.ManufacturerId))
            .MustAllBeActiveManufacturers(db)
            .When(x => x.Drives is { Count: > 0 });
        RuleForEach(x => x.Drives)
            .SetValidator(new StorageDriveFieldsValidator<CreateStorageDriveItem>());
    }
}
