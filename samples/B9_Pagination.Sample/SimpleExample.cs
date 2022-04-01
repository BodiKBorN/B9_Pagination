using System.Text.Json;
using B9_Pagination.Interfaces;
namespace B9_Pagination.Sample;

public static class SimpleExample
{
    public static async Task<IPagination<User>> GetUsersAsync(IQueryable<User> itemsQuery, PaginationQuery pagination)
    {
        var paginationResult = await itemsQuery
            .Where(x => x.Id > 10)
            .GetPaginationAsync(pagination);

        var totalItems = paginationResult.TotalItems; //29
        var pageSize = paginationResult.PageSize;     //10

        var serializeResult = JsonSerializer.Serialize(paginationResult);
        Console.WriteLine(serializeResult);

        return paginationResult;
    }

    public static async Task<IPagination<User>> GetUsersManualAsync(IQueryable<User> itemsQuery, PaginationQuery pagination)
    {
        itemsQuery = itemsQuery
            .Where(x => x.Id > 10);
        
        var pager = await itemsQuery.GetPagerAsync<User>(pagination);
        var items = await pager.GetPaginatedItemsAsync(itemsQuery);

        // You can do something with data (Select,GroupBy,etc) as IEnumerable
        var groupedItems = items
            .GroupBy(user => user.Id)
            .Select(g => new User(g.Key));

        var paginationResult = pager.GetPagination(groupedItems);

        var totalItems = paginationResult.TotalItems; //29
        var pageSize = paginationResult.PageSize;     //10

        var serializeResult = JsonSerializer.Serialize(paginationResult);
        Console.WriteLine(serializeResult);

        return paginationResult;
    }
}