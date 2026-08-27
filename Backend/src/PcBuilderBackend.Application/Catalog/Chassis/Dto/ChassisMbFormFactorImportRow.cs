using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis.Dto;

public class ChassisMbFormFactorImportRow
{
    public int ParentRowNumber { get; init; }
    public MbFormFactor MbFormFactor { get; init; }
}
