using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;

public record GraphicsCardPowerConnectorDto(PsuCableType PsuCableType, int ConnectorCount);
