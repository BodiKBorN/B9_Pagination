using B9_Pagination;
using B9_Pagination.Sample;
using MockQueryable.Moq;

var userRepositoryMock = Enumerable.Range(1, 39)
    .Select(x => new User(x))
    .AsQueryable()
    .BuildMockDbSet();

var items = userRepositoryMock.Object.AsQueryable();

var pagination = new PaginationQuery(1, 10);

await SimpleExample.GetUsersAsync(items, pagination);
await SimpleExample.GetUsersManualAsync(items, pagination);