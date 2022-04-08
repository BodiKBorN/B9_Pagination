using B9_Pagination.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace B9_Pagination.Queryable
{
    public static class PaginationExtensions
    {
        /// <summary>
        /// Gets paginated items in the same type.
        /// Before use Pagination need ordered items!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="pagination"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<IPagination<T>> GetPaginationAsync<T>(this IQueryable<T> items,
            PaginationQuery pagination,
            CancellationToken cancellationToken = default)
            where T : class
        {
            pagination ??= PaginationQuery.Default;
            return items.GetPaginationAsync(pagination.PageNumber, pagination.PageSize, cancellationToken);
        }

        /// <summary>
        /// Gets paginated items in the same type.
        /// Before use Pagination need ordered items!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<IPagination<T>> GetPaginationAsync<T>(this IQueryable<T> items,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
            where T : class
        {
            var pager = await items.GetPagerAsync<T>(pageNumber, pageSize, cancellationToken);

            if (!pager.IsValidPage)
                return pager.GetEmptyPagination();

            var result = items.Page(pager.CurrentItemNumber, pager.PageSize).ToArray();

            return pager.GetPagination(result);
        }

        /// <summary>
        /// Gets Pager with type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="pagination"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<Pager<T>> GetPagerAsync<T>(this IQueryable<object> items,
            PaginationQuery pagination,
            CancellationToken cancellationToken = default)
            where T : class
        {
            pagination ??= PaginationQuery.Default;
            return items.GetPagerAsync<T>(pagination.PageNumber, pagination.PageSize, cancellationToken);
        }

        /// <summary>
        /// Gets Pager with type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<Pager<T>> GetPagerAsync<T>(this IQueryable<object> items,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
            where T : class
            => new(await items.CountAsync(cancellationToken), pageNumber, pageSize);

        /// <summary>
        /// Gets paginated items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public static IQueryable<T> Page<T>(this IQueryable<T> items, int skip, int take)
            => items.Skip(skip).Take(take);
    }
}