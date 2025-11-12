namespace Semogly.Core.Domain.SharedContext.Results;

public class PaginationResult<T>(int page, int pageSize, int total, int totalPages, IEnumerable<T> items)
{
    public int Page { get; set; } = page;
    public int PageSize { get; set; } = pageSize;
    public int Total { get; set; } = total;
    public int TotalPages { get; set; } = totalPages;
    public IEnumerable<T> Items { get; set; } = items;
}