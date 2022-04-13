using AutoMapper;
using B9_Pagination.Abstractions;

namespace B9_Pagination.Queryable.Extensions.AutoMapper;

public static class PaginationExtensions
{
    /// <summary>
    /// Gets paginated items and map to another type if need.
    /// Before use Pagination need ordered items!
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="items"></param>
    /// <param name="pagination"></param>
    /// <param name="mapper"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task<IPagination<TResult>> GetPaginationWithMapAsync<TResult>(this IQueryable<object> items,
        PaginationQuery pagination,
        IMapper mapper,
        CancellationToken cancellationToken = default)
        where TResult : class
    {
        pagination ??= PaginationQuery.Default;
        return GetPaginationWithMapAsync<TResult>(items, pagination.PageNumber, pagination.PageSize, mapper, cancellationToken);
    }

    /// <summary>
    /// Gets paginated items and map to another type if need.
    /// Before use Pagination need ordered items!
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="items"></param>
    /// <param name="pagination"></param>
    /// <param name="configurationProvider"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<IPagination<TResult>> GetPaginationWithMapAsync<TResult>(this IQueryable<object> items,
        PaginationQuery pagination,
        IConfigurationProvider configurationProvider,
        CancellationToken cancellationToken = default)
        where TResult : class
    {
        pagination ??= PaginationQuery.Default;

        var pager = await items.GetPagerAsync<TResult>(pagination, cancellationToken);

        if (!pager.IsValidPage)
            return pager.GetEmptyPagination();

        var result = items.Page(pager.CurrentItemNumber, pager.PageSize).ToArray();

        if (result.GetType() != typeof(TResult))
            return pager.GetPagination(result as IEnumerable<TResult>);

        if (configurationProvider is null)
            throw new ArgumentNullException(nameof(configurationProvider));

        var mapper = new Mapper(configurationProvider);

        return pager.GetPagination(mapper.Map<IEnumerable<TResult>>(result));
    }

    /// <summary>
    /// Gets paginated items and map to another type if need.
    /// Before use Pagination need ordered items!
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="items"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="mapper"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<IPagination<TResult>> GetPaginationWithMapAsync<TResult>(this IQueryable<object> items,
        int pageNumber,
        int pageSize,
        IMapper mapper,
        CancellationToken cancellationToken = default)
        where TResult : class
    {
        var pager = await items.GetPagerAsync<TResult>(pageNumber, pageSize, cancellationToken);

        if (!pager.IsValidPage)
            return pager.GetEmptyPagination();

        var result = items.Page(pager.CurrentItemNumber, pager.PageSize).ToArray();

        return result.GetType() != typeof(TResult)
            ? mapper != null
                ? pager.GetPagination(mapper.Map<IEnumerable<TResult>>(result))
                : null
            : pager.GetPagination(result as IEnumerable<TResult>);
    }
}