using Microsoft.AspNetCore.Mvc;
using ZadanieRekrutacyjne.Models;
using ZadanieRekrutacyjne.Services;

namespace ZadanieRekrutacyjne.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<ActionResult> GetAsync()
        {
            DownloadFileService downloadFileService = new();
            //var temp = await downloadFileService.DownloadAndSaveCsvFile<Product, ProductMap>("Products.csv", @"C:\Users\Bartek\Desktop\Nowy folder\", ";");
            //var temp2 = await downloadFileService.DownloadAndSaveCsvFile<Inventory, InventoryMap>("Inventory.csv", @"C:\Users\Bartek\Desktop\Nowy folder\", ",");
            var temp3 = await downloadFileService.DownloadAndSaveCsvFile<Price, PriceMap>("Prices.csv", @"C:\Users\Bartek\Desktop\Nowy folder\", ",", false);
            return Ok();
        }
    }
}