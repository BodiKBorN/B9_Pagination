using System.Collections.Generic;

namespace Pagination_B9.Interfaces
{
    public interface IPagination<TData> where TData : class
    {
        int PageSize { get; set; }
        int TotalItems { get; set; }
        IEnumerable<TData> Items { get; set; }
    }
}