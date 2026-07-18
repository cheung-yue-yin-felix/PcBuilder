using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class PsuCable : BaseEntity
{
    public Guid PsuId { get; set; }
    
    public PsuCableType Type { get; set; }
    
    public int CablesCount { get; set; }
    
    public int ConnectorsCount { get; set; }
}