using System.Text.Json;
using B9_Pagination;
using B9_Pagination.Sample;
using MockQueryable.Moq;

var range = Enumerable.Range(1,99)
    .Select(x => new User(x));

var items = range.AsQueryable().BuildMockDbSet().Object;

var pagination = new PaginationQuery(pageNumber: 1, pageSize: 50);

var paginationResult = await items
    .Where(x => x.Id > 30)
    .GetPaginationAsync(pagination);

var serializeResult = JsonSerializer.Serialize(paginationResult);
Console.WriteLine(serializeResult);