namespace B9_Pagination.Abstractions
{
    public interface IPagination<TData> where TData : class
    {
        int PageSize { get; set; }
        int TotalItems { get; set; }
        IEnumerable<TData> Items { get; set; }
    }
}