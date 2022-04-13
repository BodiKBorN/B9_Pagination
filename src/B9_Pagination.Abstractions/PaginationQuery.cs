using System.ComponentModel;

namespace B9_Pagination.Abstractions
{
    [Bindable(BindableSupport.Yes)]
    public record PaginationQuery
    {
        public PaginationQuery() : this(1, 20)
        { }

        public PaginationQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public static readonly PaginationQuery Default = new();

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}