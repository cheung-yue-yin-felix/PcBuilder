namespace PcBuilderBackend.Application.Common.Dto;

public record RangeFilter
{
    public decimal? Min { get; init; }
    public decimal? Max { get; init; }
}