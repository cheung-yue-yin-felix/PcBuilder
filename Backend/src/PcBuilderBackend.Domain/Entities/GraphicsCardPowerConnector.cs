using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class GraphicsCardPowerConnector : BaseEntity
{
    public Guid GraphicsCardId { get; set; }
    
    public PsuCableType PsuCableType { get; set; }
    
    public int ConnectorCount { get; set; }
}