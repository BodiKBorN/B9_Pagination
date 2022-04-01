using B9_Pagination.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
namespace B9_Pagination.APISample.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly Mock<DbSet<WeatherForecast>> _mockWeatherRepository;
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _mockWeatherRepository = Enumerable.Range(1, 31)
            .Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .AsQueryable()
            .BuildMockDbSet();
        _logger = logger;
    }

    [HttpGet("pagination-with-defaultModel")]
    public Task<IPagination<WeatherForecast>> GetPaginatedWeatherForecastDefault(
        [FromQuery] PaginationQuery pagination,
        CancellationToken cancellationToken = default)
        => _mockWeatherRepository.Object.AsQueryable()
            .GetPaginationAsync(pagination, cancellationToken);

    [HttpGet("pagination")]
    public Task<IPagination<WeatherForecast>> GetPaginatedWeatherForecast(
        int pageNumber = 1,
        int pageSize = 5,
        CancellationToken cancellationToken = default)
        => _mockWeatherRepository.Object.AsQueryable()
            .GetPaginationAsync(pageNumber, pageSize, cancellationToken);
}