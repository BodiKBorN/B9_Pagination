namespace B9_Pagination.Abstractions
{
    public interface IPagination<out TData> where TData : class
    {
        int PageSize { get; }
        int TotalItems { get; }
        IEnumerable<TData> Items { get; }
    }
}