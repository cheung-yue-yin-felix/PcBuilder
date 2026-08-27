using FluentValidation;
using PcBuilderBackend.Application.Catalog.StorageDrives.Commands.UpdateStorageDrive;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Validators;

public class UpdateStorageDriveCommandValidator : AbstractValidator<UpdateStorageDriveCommand>
{
    public UpdateStorageDriveCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ManufacturerId).NotEmpty();
        RuleFor(x => x.Media).IsInEnum();
        RuleFor(x => x.Interface).IsInEnum();
        RuleFor(x => x.FormFactor).IsInEnum();
        RuleFor(x => x.CapacityGb).GreaterThan(0);

        When(x => x.FormFactor is StorageFormFactor.Sata25 or StorageFormFactor.Sata35, () =>
        {
            RuleFor(x => x.Media)
                .Equal(StorageMedia.Hdd)
                .WithMessage("Storage media must be HDD for the selected form factor.");
            RuleFor(x => x.Rpm)
                .NotNull()
                .GreaterThan(0)
                .WithMessage("RPM is required for HDD.");
        });

        When(x => x.FormFactor is StorageFormFactor.M22230 or StorageFormFactor.M22242
            or StorageFormFactor.M22260 or StorageFormFactor.M22280 or StorageFormFactor.M222110, () =>
        {
            RuleFor(x => x.Media)
                .Equal(StorageMedia.Ssd)
                .WithMessage("Storage media must be SSD for the selected form factor.");
            RuleFor(x => x.PcieGeneration)
                .NotNull()
                .IsInEnum()
                .WithMessage("PCIe generation is required for SSD.");
        });
    }
}
