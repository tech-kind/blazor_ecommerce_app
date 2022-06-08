using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using BlazorECommerceApp.Shared;

namespace BlazorECommerceApp.Server.Controllers;

// Authorize�����ŔF�؂��K�v�ł��邱�Ƃ������i�P�Ȃ�ڈ�ł͂���j
// RequiredScope���������邱�ƂŁA�K�؂ȃX�R�[�v����Ăяo����Ă��邩���`�F�b�N����
// RequiredScopesConfigurationKey�łǂ̃X�R�[�v�ł��邩���w�肷��
// ���̗�ł́Aappsettings.json��Scopes���Q�Ƃ��Ă���
[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAdB2C:Scopes")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
