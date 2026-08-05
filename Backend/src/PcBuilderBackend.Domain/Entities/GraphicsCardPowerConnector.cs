using System.Data;
using System.Runtime.Serialization;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class GraphicsCardPowerConnector : BaseEntity
{
    public Guid GraphicsCardId { get; set; }
    public PsuCableType PsuCableType { get; set; }
    public int ConnectorCount { get; set; }
    
    protected GraphicsCardPowerConnector() {}

    public GraphicsCardPowerConnector(Guid graphicsCardId, PsuCableType psuCableType, int connectorCount)
    {
        SetSpecs(graphicsCardId, psuCableType, connectorCount);
    }

    public void UpdateSpecs(Guid graphicsCardId, PsuCableType type, int connectorsCount)
    {
        SetSpecs(graphicsCardId, type, connectorsCount);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void SetSpecs(Guid graphicsCardId, PsuCableType psuCableType, int connectorCount)
    {
        if (graphicsCardId == Guid.Empty)
            throw new ArgumentException("Graphics Card ID cannot be empty");
        
        if (!Enum.IsDefined(psuCableType))
            throw new ArgumentException("PSU Cable Type is invalid");
        
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(connectorCount);
        
        GraphicsCardId = graphicsCardId;
        PsuCableType = psuCableType;
        ConnectorCount = connectorCount;
    }
}
