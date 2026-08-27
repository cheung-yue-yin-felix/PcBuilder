using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis.Dto;

public class ChassisPsuFormFactorImportRow
{
    public int ParentRowNumber { get; init; }
    public PsuFormFactor PsuFormFactor { get; init; }
}
