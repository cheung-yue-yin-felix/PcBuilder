using FluentValidation;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Commands.BulkCreateWirelessNetworkAdapters;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Validators;

public class BulkCreateWirelessNetworkAdaptersCommandValidator
    : AbstractValidator<BulkCreateWirelessNetworkAdaptersCommand>
{
    public BulkCreateWirelessNetworkAdaptersCommandValidator(IActiveEntityLookup db)
    {
        RuleFor(x => x.Adapters).NotEmpty();
        RuleFor(x => x.Adapters.Select(a => a.ManufacturerId))
            .MustAllBeActiveManufacturers(db)
            .When(x => x.Adapters is { Count: > 0 });
        RuleForEach(x => x.Adapters).ChildRules(adapter =>
        {
            adapter.RuleFor(a => a.Name).NotEmpty().MaximumLength(200);
            adapter.RuleFor(a => a.ManufacturerId).NotEmpty();
            adapter.RuleFor(a => a.WifiStandard).IsInEnum();
            adapter.RuleFor(a => a.HostInterface).IsInEnum();
            adapter.RuleFor(a => a.MaxSpeedMbps).GreaterThan(0);
            adapter.RuleFor(a => a.MaxSpeedMbps5G).GreaterThan(0).When(a => a.MaxSpeedMbps5G.HasValue);
            adapter.RuleFor(a => a.MaxSpeedMbps6G).GreaterThan(0).When(a => a.MaxSpeedMbps6G.HasValue);
            adapter.RuleFor(a => a.BluetoothVersion).IsInEnum().When(a => a.BluetoothVersion.HasValue);

            adapter.When(a => a.HostInterface == WirelessHostInterface.M2, () =>
            {
                adapter.RuleFor(a => a.Key)
                    .Equal(M2Key.E)
                    .WithMessage("M.2 wireless adapters must use E-key.");
                adapter.RuleFor(a => a.M2FormFactor)
                    .NotNull()
                    .IsInEnum()
                    .WithMessage("M.2 form factor is required for M.2 wireless adapters.");
                adapter.RuleFor(a => a.PcieSlotType)
                    .Null()
                    .WithMessage("PCIe slot type is not valid for M.2 wireless adapters.");
                adapter.RuleFor(a => a.UsbVersion)
                    .Null()
                    .WithMessage("USB fields are not valid for M.2 wireless adapters.");
                adapter.RuleFor(a => a.UsbType)
                    .Null()
                    .WithMessage("USB fields are not valid for M.2 wireless adapters.");
            });

            adapter.When(a => a.HostInterface == WirelessHostInterface.Pcie, () =>
            {
                adapter.RuleFor(a => a.PcieSlotType)
                    .NotNull()
                    .IsInEnum()
                    .WithMessage("PCIe slot type is required for PCIe wireless adapters.");
                adapter.RuleFor(a => a.Key)
                    .Null()
                    .WithMessage("M.2 key and form factor are not valid for PCIe wireless adapters.");
                adapter.RuleFor(a => a.M2FormFactor)
                    .Null()
                    .WithMessage("M.2 key and form factor are not valid for PCIe wireless adapters.");
                adapter.RuleFor(a => a.UsbVersion)
                    .Null()
                    .WithMessage("USB fields are not valid for PCIe wireless adapters.");
                adapter.RuleFor(a => a.UsbType)
                    .Null()
                    .WithMessage("USB fields are not valid for PCIe wireless adapters.");
            });

            adapter.When(a => a.HostInterface == WirelessHostInterface.Usb, () =>
            {
                adapter.RuleFor(a => a.UsbVersion)
                    .NotNull()
                    .IsInEnum()
                    .WithMessage("USB version is required for USB wireless adapters.");
                adapter.RuleFor(a => a.UsbType)
                    .NotNull()
                    .IsInEnum()
                    .WithMessage("USB type is required for USB wireless adapters.");
                adapter.RuleFor(a => a.PcieSlotType)
                    .Null()
                    .WithMessage("USB wireless adapters cannot have PCIe or M.2 fields.");
                adapter.RuleFor(a => a.Key)
                    .Null()
                    .WithMessage("USB wireless adapters cannot have PCIe or M.2 fields.");
                adapter.RuleFor(a => a.M2FormFactor)
                    .Null()
                    .WithMessage("USB wireless adapters cannot have PCIe or M.2 fields.");
            });
        });
    }
}
