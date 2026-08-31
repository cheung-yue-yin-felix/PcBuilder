using FluentValidation;
using PcBuilderBackend.Application.Catalog.StorageDrives.Commands.CreateStorageDrive;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.StorageDrives.Validators;

public class CreateStorageDriveCommandValidator : AbstractValidator<CreateStorageDriveCommand>
{
    public CreateStorageDriveCommandValidator(IActiveEntityLookup db)
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ManufacturerId).NotEmpty().MustBeActiveManufacturer(db);
        RuleFor(x => x.Media).IsInEnum();
        RuleFor(x => x.Interface).IsInEnum();
        RuleFor(x => x.FormFactor).IsInEnum();
        RuleFor(x => x.CapacityGb).GreaterThan(0);

        When(x => x.FormFactor is StorageFormFactor.Sata35, () =>
        {
            RuleFor(x => x.Media)
                .Equal(StorageMedia.Hdd)
                .WithMessage("Storage media must be HDD for the selected form factor.");
            RuleFor(x => x.Rpm)
                .NotNull()
                .GreaterThan(0)
                .WithMessage("RPM is required for HDD.");
        });

        When(x => x.FormFactor is StorageFormFactor.Sata25 && x.Media == StorageMedia.Hdd, () =>
        {
            RuleFor(x => x.Rpm)
                .NotNull()
                .GreaterThan(0)
                .WithMessage("RPM is required for HDD.");
        });

        When(x => x.FormFactor is StorageFormFactor.Sata25 && x.Media == StorageMedia.Ssd, () =>
        {
            RuleFor(x => x.Interface)
                .Equal(StorageInterface.Sata)
                .WithMessage("2.5\" SSD must use a SATA interface.");
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
