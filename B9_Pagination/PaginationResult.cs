using System.Collections.Generic;
using B9_Pagination.Interfaces;

namespace B9_Pagination
{
    public class PaginationResult<TData> : IPagination<TData> where TData : class
    {
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public IEnumerable<TData> Items { get; set; }
    }
}