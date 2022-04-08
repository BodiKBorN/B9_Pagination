using Microsoft.EntityFrameworkCore;

namespace B9_Pagination.Queryable;

public static class PagerExtensions
{
    public static async Task<IEnumerable<T>> GetPaginatedItemsAsync<T>(this Pager<T> pager,
        IQueryable<T> items,
        CancellationToken cancellationToken = default)
        where T : class
    {
        if (!pager.IsValidPage)
            return Enumerable.Empty<T>();

        return await items.Page(pager.CurrentItemNumber, pager.PageSize).ToArrayAsync(cancellationToken);
    }
}