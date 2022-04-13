namespace B9_Pagination.Abstractions
{
    public record Pagination<TData> : IPagination<TData> where TData : class
    {
        public Pagination(int pageSize, int totalItems, IEnumerable<TData> items)
        {
            PageSize = pageSize;
            TotalItems = totalItems;
            Items = items;
        }
        public int PageSize { get; }
        public int TotalItems { get; }
        public IEnumerable<TData> Items { get; }
    }
}