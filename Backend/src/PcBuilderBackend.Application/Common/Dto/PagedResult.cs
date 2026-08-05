namespace PcBuilderBackend.Application.Common.Dto;

public record PagedResult<T>
{
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public List<T> Items { get; init; } = new();
}