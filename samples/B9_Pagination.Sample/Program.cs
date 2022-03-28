using System.Text.Json;
using B9_Pagination;
using B9_Pagination.Sample;
using MockQueryable.Moq;

var userRepositoryMock = Enumerable.Range(1, 39)
    .Select(x => new User(x))
    .AsQueryable()
    .BuildMockDbSet();

var items = userRepositoryMock.Object;

var pagination = new PaginationQuery(pageNumber: 1, pageSize: 10);

var paginationResult = await items
    .Where(x => x.Id > 10)
    .GetPaginationAsync(pagination);

var totalItems = paginationResult.TotalItems; //29
var pageSize = paginationResult.PageSize;     //10

var serializeResult = JsonSerializer.Serialize(paginationResult);
Console.WriteLine(serializeResult);