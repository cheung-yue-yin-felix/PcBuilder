using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class Gpu : BaseEntity
{
    public Guid ManufacturerId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Series { get; set; } = string.Empty;
}