using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pagination_B9.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B9_Pagination
{
    public static class PaginationExtension
    {
        /// <summary>
        /// Gets paginated items and map to another type if need.
        /// Before use Pagination need ordered items!
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="items"></param>
        /// <param name="pagination"></param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public static Task<IPagination<TResult>> GetPaginationWithMapAsync<TResult>(this IQueryable<object> items, PaginationQuery pagination, IMapper mapper) where TResult : class
        {
            pagination ??= PaginationQuery.Default;
            return GetPaginationWithMapAsync<TResult>(items, pagination.PageNumber, pagination.PageSize, mapper);
        }

        /// <summary>
        /// Gets paginated items and map to another type if need.
        /// Before use Pagination need ordered items!
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="items"></param>
        /// <param name="pagination"></param>
        /// <param name="configurationProvider"></param>
        /// <returns></returns>
        public static async Task<IPagination<TResult>> GetPaginationWithMapAsync<TResult>(this IQueryable<object> items, PaginationQuery pagination, IConfigurationProvider configurationProvider) where TResult : class
        {
            pagination ??= PaginationQuery.Default;

            var pager = await items.GetPagerAsync<TResult>(pagination);

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
        /// <returns></returns>
        public static async Task<IPagination<TResult>> GetPaginationWithMapAsync<TResult>(this IQueryable<object> items, int pageNumber, int pageSize, IMapper mapper) where TResult : class
        {
            var pager = await items.GetPagerAsync<TResult>(pageNumber, pageSize);

            if (!pager.IsValidPage)
                return pager.GetEmptyPagination();

            var result = items.Page(pager.CurrentItemNumber, pager.PageSize).ToArray();

            return result.GetType() != typeof(TResult)
                ? mapper != null
                    ? pager.GetPagination(mapper.Map<IEnumerable<TResult>>(result))
                    : null
                : pager.GetPagination(result as IEnumerable<TResult>);
        }

        /// <summary>
        /// Gets paginated items in the same type.
        /// Before use Pagination need ordered items!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public static Task<IPagination<T>> GetPaginationAsync<T>(this IQueryable<T> items, PaginationQuery pagination) where T : class
        {
            pagination ??= PaginationQuery.Default;
            return items.GetPaginationAsync<T>(pagination.PageNumber, pagination.PageSize);
        }

        /// <summary>
        /// Gets paginated items in the same type.
        /// Before use Pagination need ordered items!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<IPagination<T>> GetPaginationAsync<T>(this IQueryable<T> items, int pageNumber, int pageSize) where T : class
        {
            var pager = await items.GetPagerAsync<T>(pageNumber, pageSize);

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
        /// <returns></returns>
        public static Task<Pager<T>> GetPagerAsync<T>(this IQueryable<object> items, PaginationQuery pagination) where T : class
        {
            pagination ??= PaginationQuery.Default;
            return items.GetPagerAsync<T>(pagination.PageNumber, pagination.PageSize);
        }

        /// <summary>
        /// Gets Pager with type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<Pager<T>> GetPagerAsync<T>(this IQueryable<object> items, int pageNumber, int pageSize) where T : class
          => new Pager<T>(await items.CountAsync(), pageNumber, pageSize);

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