using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class StorageDrive : ProductEntity
{
    public StorageMedia Media { get; private set; }
    public StorageInterface Interface { get; private set; }
    public StorageFormFactor FormFactor { get; private set; }
    public int CapacityGb { get; private set; }
    public PcieGeneration? PcieGeneration { get; private set; }
    public int? Rpm { get; private set; }

    public bool IsM2 => FormFactor is
        StorageFormFactor.M22230 or
        StorageFormFactor.M22242 or
        StorageFormFactor.M22260 or
        StorageFormFactor.M22280 or
        StorageFormFactor.M222110;

    public M2Key? ModuleKey =>
        !IsM2
            ? null
            : Interface == StorageInterface.Sata
                ? M2Key.BM
                : M2Key.M;

    public M2FormFactor? M2FormFactor =>
        IsM2 ? (M2FormFactor)(int)FormFactor : null;
    
    protected StorageDrive() {}

    public StorageDrive(
        string name,
        Guid manufacturerId,
        StorageMedia storageMedia,
        StorageInterface storageInterface,
        StorageFormFactor factor,
        int capacityGb,
        PcieGeneration? pcieGeneration = null,
        int? rpm = null)
    {
        SetName(name);
        SetManufacturer(manufacturerId);
        SetSpecs(storageMedia, storageInterface, factor, capacityGb, pcieGeneration, rpm);
    }

    public void UpdateSpecs(StorageMedia storageMedia, StorageInterface storageInterface, StorageFormFactor factor,
        int capacityGb, PcieGeneration? pcieGeneration = null, int? rpm = null)
    {
        SetSpecs(storageMedia, storageInterface, factor, capacityGb, pcieGeneration, rpm);
        UpdatedAtUtc = DateTime.UtcNow;
    }
    
    private void SetSpecs(StorageMedia storageMedia, StorageInterface storageInterface, StorageFormFactor factor, int capacityGb, PcieGeneration? pcieGeneration = null, int? rpm = null)
    {
        var ssdForms = new[]
        {
            StorageFormFactor.M22230, StorageFormFactor.M22242, StorageFormFactor.M22260, StorageFormFactor.M22280,
            StorageFormFactor.M222110
        };

        var hddForms = new[]
        {
            StorageFormFactor.Sata25, StorageFormFactor.Sata35
        };

        if (hddForms.Contains(factor))
        {
            if (storageMedia != StorageMedia.Hdd)
                throw new ArgumentException("Storage media must be HDD for the selected form factor.", nameof(storageMedia));

            if (!rpm.HasValue)
                throw new ArgumentException("RPM is required for HDD.", nameof(rpm));

            SetHddSpecs(storageMedia, storageInterface, factor, capacityGb, rpm.Value);
        }
        else if (ssdForms.Contains(factor))
        {
            if (storageMedia != StorageMedia.Ssd)
                throw new ArgumentException("Storage media must be SSD for the selected form factor.", nameof(storageMedia));

            if (!pcieGeneration.HasValue)
                throw new ArgumentException("PCIe generation is required for SSD.", nameof(pcieGeneration));

            SetSsdSpecs(storageMedia, storageInterface, factor, capacityGb, pcieGeneration.Value);
        }
        else
        {
            throw new ArgumentException("Storage form factor is not recognized as HDD or SSD.", nameof(factor));
        }
    }

    private void SetHddSpecs(StorageMedia storageMedia, StorageInterface storageInterface, StorageFormFactor factor,
        int capacityGb, int rpm)
    {
        if (!Enum.IsDefined(storageInterface))
            throw new ArgumentException("Storage Interface is invalid.");
        
        if (!Enum.IsDefined(storageMedia))
            throw new ArgumentException("Storage Media is invalid.");
        
        if (!Enum.IsDefined(factor))
            throw new ArgumentException("Storage Form Factor is invalid.");
        
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(capacityGb);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rpm);
        
        Interface = storageInterface;
        Media = storageMedia;
        FormFactor = factor;
        CapacityGb = capacityGb;
        Rpm = rpm;
        PcieGeneration = null;
    }

    private void SetSsdSpecs(StorageMedia storageMedia, StorageInterface storageInterface, StorageFormFactor factor,
        int capacityGb, PcieGeneration pcieGeneration)
    {
        if (!Enum.IsDefined(storageMedia))
            throw new ArgumentException("Storage Media is invalid.");
            
        if (!Enum.IsDefined(storageInterface))
            throw new ArgumentException("Storage Interface is invalid.");
        
        if (!Enum.IsDefined(factor))
            throw new ArgumentException("Storage Form Factor is invalid.");
        
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(capacityGb);
        
        if (!Enum.IsDefined(pcieGeneration))
            throw new ArgumentException("PcieGeneration is invalid.");
        
        Interface = storageInterface;
        Media = storageMedia;
        FormFactor = factor;
        CapacityGb = capacityGb;
        PcieGeneration = pcieGeneration;
        Rpm = null;
    }
}
