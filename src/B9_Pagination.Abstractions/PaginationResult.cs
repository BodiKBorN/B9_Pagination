namespace B9_Pagination.Abstractions
{
    public record PaginationResult<TData> : IPagination<TData> where TData : class
    {
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public IEnumerable<TData> Items { get; set; }
    }
}