using FluentValidation;
using PcBuilderBackend.Domain.Enums;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Validators;

public sealed class WirelessNetworkAdapterFieldsValidator<T> : AbstractValidator<T>
    where T : IWirelessNetworkAdapterFields
{
    public WirelessNetworkAdapterFieldsValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ManufacturerId).NotEmpty();
        RuleFor(x => x.WifiStandard).IsInEnum();
        RuleFor(x => x.HostInterface).IsInEnum();
        RuleFor(x => x.MaxSpeedMbps).GreaterThan(0);
        RuleFor(x => x.MaxSpeedMbps5G).GreaterThan(0).When(x => x.MaxSpeedMbps5G.HasValue);
        RuleFor(x => x.MaxSpeedMbps6G).GreaterThan(0).When(x => x.MaxSpeedMbps6G.HasValue);
        RuleFor(x => x.BluetoothVersion).IsInEnum().When(x => x.BluetoothVersion.HasValue);

        When(x => x.HostInterface == WirelessHostInterface.M2, () =>
        {
            RuleFor(x => x.Key)
                .Equal(M2Key.E)
                .WithMessage("M.2 wireless adapters must use E-key.");
            RuleFor(x => x.M2FormFactor)
                .NotNull()
                .IsInEnum()
                .WithMessage("M.2 form factor is required for M.2 wireless adapters.");
            RuleFor(x => x.PcieSlotType)
                .Null()
                .WithMessage("PCIe slot type is not valid for M.2 wireless adapters.");
            RuleFor(x => x.UsbVersion)
                .Null()
                .WithMessage("USB fields are not valid for M.2 wireless adapters.");
            RuleFor(x => x.UsbType)
                .Null()
                .WithMessage("USB fields are not valid for M.2 wireless adapters.");
        });

        When(x => x.HostInterface == WirelessHostInterface.Pcie, () =>
        {
            RuleFor(x => x.PcieSlotType)
                .NotNull()
                .IsInEnum()
                .WithMessage("PCIe slot type is required for PCIe wireless adapters.");
            RuleFor(x => x.Key)
                .Null()
                .WithMessage("M.2 key and form factor are not valid for PCIe wireless adapters.");
            RuleFor(x => x.M2FormFactor)
                .Null()
                .WithMessage("M.2 key and form factor are not valid for PCIe wireless adapters.");
            RuleFor(x => x.UsbVersion)
                .Null()
                .WithMessage("USB fields are not valid for PCIe wireless adapters.");
            RuleFor(x => x.UsbType)
                .Null()
                .WithMessage("USB fields are not valid for PCIe wireless adapters.");
        });

        When(x => x.HostInterface == WirelessHostInterface.Usb, () =>
        {
            RuleFor(x => x.UsbVersion)
                .NotNull()
                .IsInEnum()
                .WithMessage("USB version is required for USB wireless adapters.");
            RuleFor(x => x.UsbType)
                .NotNull()
                .IsInEnum()
                .WithMessage("USB type is required for USB wireless adapters.");
            RuleFor(x => x.PcieSlotType)
                .Null()
                .WithMessage("USB wireless adapters cannot have PCIe or M.2 fields.");
            RuleFor(x => x.Key)
                .Null()
                .WithMessage("USB wireless adapters cannot have PCIe or M.2 fields.");
            RuleFor(x => x.M2FormFactor)
                .Null()
                .WithMessage("USB wireless adapters cannot have PCIe or M.2 fields.");
        });
    }
}
