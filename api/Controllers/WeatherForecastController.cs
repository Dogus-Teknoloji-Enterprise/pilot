using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class WeatherForecastController : ControllerBase
    {
        string Password = "admin";

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost(Name = "PostWeatherForecast")]
        public string Post(WeatherForecast weatherForecast)
        {
            var username = "admin";

            MD5 md5 = MD5.Create();
            var rng = new Random();
            var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(Password));

            var adminName = encryptString(username);

            var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{adminName}:{hash}"));
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64String);

            var response = client.PostAsJsonAsync("https://localhost:5001/WeatherForecast", weatherForecast);

            return Password;
        }
        public static byte[] encryptString(string message)
        {
            SymmetricAlgorithm serviceProvider = new DESCryptoServiceProvider();
            byte[] key = { 16, 22, 240, 11, 18, 150, 192, 21 };
            serviceProvider.Key = key;
            ICryptoTransform encryptor = serviceProvider.CreateEncryptor();

            byte[] messageB = System.Text.Encoding.ASCII.GetBytes(message);
            return encryptor.TransformFinalBlock(messageB, 0, messageB.Length);
        }
    }
}
