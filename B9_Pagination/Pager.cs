using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace B9_Pagination
{
    public class Pager<TData> where TData : class
    {
        private readonly int _pageNumber;
        private readonly int _totalPages;
        private readonly int _totalCount;

        public Pager(int totalCount, int pageNumber, int pageSize)
        {
            _totalCount = totalCount;
            _pageNumber = pageNumber;
            _totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            CurrentItemNumber = (pageNumber - 1) * pageSize;
            PageSize = pageSize;
        }

        public int CurrentItemNumber { get; }
        public int PageSize { get; }
        public bool IsValidPage => _pageNumber > 0 && _pageNumber <= _totalPages;

        public PaginationResult<TData> GetEmptyPagination()
            => new()
            {
                PageSize = PageSize,
                TotalItems = 0,
                Items = Enumerable.Empty<TData>()
            };

        public PaginationResult<TData> GetPagination(IList<TData> items)
            => new()
            {
                PageSize = PageSize,
                TotalItems = _totalCount,
                Items = items
            };

        public PaginationResult<TData> GetPagination(IEnumerable<TData> items)
            => new()
            {
                PageSize = PageSize,
                TotalItems = _totalCount,
                Items = items
            };

        public async Task<IEnumerable<T>> GetPaginatedItemsAsync<T>(IQueryable<T> items) where T : class
        {
            if (!IsValidPage)
                return Enumerable.Empty<T>();

            return await items.Page(CurrentItemNumber, PageSize).ToArrayAsync();
        }
    }
}