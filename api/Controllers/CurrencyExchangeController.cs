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

        [HttpGet("currency-exchange")]
        public async Task<IActionResult> GetExchangeRates()
        {
            var response = await _httpClient.GetAsync("https://api.exchangeratesapi.io/latest");
            if (response.IsSuccessStatusCode)
            {
                var exchangeRates = await response.Content.ReadAsStringAsync();
                return Ok(exchangeRates);
            }
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }
    }
}
