# B9_Pagination
[![Version](https://img.shields.io/nuget/v/B9_Pagination?style=plastic)](https://www.nuget.org/packages/B9_Pagination)
[![Downloads](https://img.shields.io/nuget/dt/B9_pagination?style=plastic)](https://www.nuget.org/packages/B9_Pagination)

Extensions for simple pagination

## How to use?
You can use PaginationQuery model for set pagination parameter
```
var pagination = new PaginationQuery(pageNumber: 1, pageSize: 10);

var paginationResult = await items
    .Where(x => x.Id > 10)
    .GetPaginationAsync(pagination);
```
or set directly in method
```
var paginationResult = await items
    .Where(x => x.Id > 10)
    .GetPaginationAsync(pageNumber: 1, pageSize: 10);
```
*Check out the [sample project](https://github.com/BodiKBorN/B9_Pagination/tree/dev/samples/B9_Pagination.Sample)*

You can use pagination in API endpoints
```
[HttpGet]
[ProducesResponseType(typeof(IPagination<GetShortAccountDto>), StatusCodes.Status200OK)]
public async Task<IActionResult> GetClientAccounts(
       [FromQuery] PaginationQuery pagination = default,
       [FromQuery] FilterDto filter = default,
       UserEnums.SortBy? sortBy = default,
       CancellationToken cancellationToken = default)
       => (await _accountService.GetAccountsAsync(pagination, filter, sortBy, cancellationToken)).ToActionResult();
```
*Check out the [sample project](https://github.com/BodiKBorN/B9_Pagination/tree/dev/samples/B9_Pagination.APISample)*

#### What if you need to do something(Select,Where,GroupBy,etc) with the data before doing pagination?

You can use `Pager<T>` and manage validation yourself
```
var mappedItems = items
    .GroupBy(dto => dto.Name)
    .Select(g => new UserDto
    {
       Name = g.Key,
       Contacts = g.Select(dto => new UserContactDto
       {
          Email = dto.Email,
          Count = dto.Count
       })
     })
     .ToList();

var totalCount = mappedItems.Count();
var pager = new Pager<UserDto>(totalCount, pagination.PageNumber, pagination.PageSize);

if (!pager.IsValidPage)
       return pager.GetEmptyPagination();

return pager.GetPagination(mappedItems);
```

Additionaly if you need just map type you can use extension methods for map with **[AutoMapper](https://github.com/AutoMapper/AutoMapper)**
`GetPaginationWithMapAsync(this IQueryable<object> items, PaginationQuery pagination, IMapper mapper, CancellationToken cancellationToken = default)`
or
`GetPaginationWithMapAsync<TResult>(this IQueryable<object> items, PaginationQuery pagination, IConfigurationProvider configurationProvider, CancellationToken cancellationToken = default)`
