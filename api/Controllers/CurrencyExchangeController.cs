using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyExchangeController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public CurrencyExchangeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetExchangeRate(string baseCurrency, string targetCurrency)
        {
            var response = await _httpClient.GetAsync($"https://api.exchangeratesapi.io/latest?base={baseCurrency}&symbols={targetCurrency}");
            if (response.IsSuccessStatusCode)
            {
                var exchangeRate = await response.Content.ReadAsStringAsync();
                return Ok(exchangeRate);
            }
            return BadRequest("Error retrieving exchange rate.");
        }

        [HttpGet("historical")]
        public async Task<IActionResult> GetHistoricalExchangeRates(string baseCurrency, string targetCurrency, string startDate, string endDate)
        {
            var response = await _httpClient.GetAsync($"https://api.exchangeratesapi.io/history?start_at={startDate}&end_at={endDate}&base={baseCurrency}&symbols={targetCurrency}");
            if (response.IsSuccessStatusCode)
            {
                var historicalRates = await response.Content.ReadAsStringAsync();
                return Ok(historicalRates);
            }
            return BadRequest("Error retrieving historical exchange rates.");
        }
    }
}
