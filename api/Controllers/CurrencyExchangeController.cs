using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyExchangeController : ControllerBase
    {
        private static readonly Dictionary<string, decimal> TodaysRates = new Dictionary<string, decimal>
        {
            { "USD", 1.0m },
            { "EUR", 0.85m },
            { "GBP", 0.75m },
            { "JPY", 110.0m }
        };

        private static readonly Dictionary<DateTime, Dictionary<string, decimal>> HistoricalRates = new Dictionary<DateTime, Dictionary<string, decimal>>
        {
            { DateTime.Parse("2021-09-01"), new Dictionary<string, decimal> { { "USD", 1.0m }, { "EUR", 0.84m }, { "GBP", 0.74m }, { "JPY", 109.0m } } },
            { DateTime.Parse("2021-09-02"), new Dictionary<string, decimal> { { "USD", 1.0m }, { "EUR", 0.85m }, { "GBP", 0.75m }, { "JPY", 110.0m } } }
        };

        [HttpGet("today")]
        public IActionResult GetTodaysRates()
        {
            return Ok(TodaysRates);
        }

        [HttpGet("historical")]
        public IActionResult GetHistoricalRates([FromQuery] string date)
        {
            if (DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                if (HistoricalRates.TryGetValue(parsedDate, out var rates))
                {
                    return Ok(rates);
                }
                else
                {
                    return NotFound("Rates not found for the specified date.");
                }
            }
            else
            {
                return BadRequest("Invalid date format. Please use yyyy-MM-dd.");
            }
        }
    }
}
