using FluentValidation;
using PcBuilderBackend.Application.Catalog.StorageDrives.Commands.CreateStorageDrive;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Validators;

public class CreateStorageDriveCommandValidator : AbstractValidator<CreateStorageDriveCommand>
{
    public CreateStorageDriveCommandValidator(IActiveEntityLookup db)
    {
        Include(new StorageDriveFieldsValidator<CreateStorageDriveCommand>());
        RuleFor(x => x.ManufacturerId).MustBeActiveManufacturer(db);
    }
}
