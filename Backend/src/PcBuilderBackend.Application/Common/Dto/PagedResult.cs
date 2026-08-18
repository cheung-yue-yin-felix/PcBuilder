namespace PcBuilderBackend.Application.Common.Dto;

public record PagedResult<T>
{
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public List<T> Items { get; init; } = new();

    public static PagedResult<T> Empty(PagedRequest request) => new()
    {
        PageIndex = request.PageIndex,
        PageSize = request.PageSize,
        TotalCount = 0,
        Items = []
    };

    public static PagedResult<T> Empty(int pageIndex, int pageSize) => new()
    {
        PageIndex = pageIndex,
        PageSize = pageSize,
        TotalCount = 0,
        Items = []
    };
}