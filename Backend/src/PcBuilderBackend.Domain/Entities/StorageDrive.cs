using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class StorageDrive : ProductEntity
{
    public StorageMedia Media { get; set; }
    public StorageInterface Interface { get; set; }
    public StorageFormFactor FormFactor { get; set; }
    public int CapacityGb { get; set; }
    public PcieGeneration? PcieGeneration { get; set; }
    public int? Rpm { get; set; }
    
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
    }
}
