using FluentValidation;
using PcBuilderBackend.Application.Catalog.StorageDrives.Commands.UpdateStorageDrive;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Validators;

public class UpdateStorageDriveCommandValidator : AbstractValidator<UpdateStorageDriveCommand>
{
    public UpdateStorageDriveCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        Include(new StorageDriveFieldsValidator<UpdateStorageDriveCommand>());
    }
}
