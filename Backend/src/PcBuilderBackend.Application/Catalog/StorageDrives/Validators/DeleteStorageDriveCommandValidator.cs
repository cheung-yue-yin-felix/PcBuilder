using FluentValidation;
using PcBuilderBackend.Application.Catalog.StorageDrives.Commands.DeleteStorageDrive;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Validators;

public class DeleteStorageDriveCommandValidator : AbstractValidator<DeleteStorageDriveCommand>
{
    public DeleteStorageDriveCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
