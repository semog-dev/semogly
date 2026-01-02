using Microsoft.EntityFrameworkCore;
using Semogly.Core.Domain.SharedContext.Results;

namespace Semogly.Core.Infrastructure.Extensions;

public static class QueriableExtensions
{
    public static async Task<PaginationResult<T>> ToPaginationResultAsync<T>(this IQueryable<T> query, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var total = query.Count();
        var totalPages = (int)Math.Ceiling((double)total / pageSize);

        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        return new PaginationResult<T>(page, pageSize, total, totalPages, items);
    }
}