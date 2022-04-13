using System;
using System.Collections.Generic;
using System.Linq;
using B9_Pagination.Abstractions;

namespace B9_Pagination
{
    public class Pager<TData> where TData : class
    {
        private readonly int _pageNumber;
        private readonly int _totalPages;
        private readonly int _totalCount;

        // TODO: protected internal constructor and add IEnumerable extensions
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

        public IPagination<TData> GetEmptyPagination()
            => new Pagination<TData>(PageSize, 0, Enumerable.Empty<TData>());

        public IPagination<TData> GetPagination(IEnumerable<TData> items)
            => new Pagination<TData>(PageSize, _totalCount, items);
    }
}