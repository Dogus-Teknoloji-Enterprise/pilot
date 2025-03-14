using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyExchangeController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetExchangeRates()
        {
            var exchangeRates = new List<object>
            {
                new { Currency = "USD", Rate = 1.0 },
                new { Currency = "EUR", Rate = 0.85 },
                new { Currency = "GBP", Rate = 0.75 }
            };

            return Ok(exchangeRates);
        }
    }
}
