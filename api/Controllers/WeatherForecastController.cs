using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class WeatherForecastController : ControllerBase
    {
        private string Ssername = "admin";
        private string Password = "password123";

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        //SQL Injection
        [HttpGet(Name = "GetUserData")]
        public void GetUserData(string userId)
        {
            string connectionString = "your_connection_string";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = $"SELECT * FROM Users WHERE UserId = '{userId}'";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(reader["Username"]);
                }
            }
        }

        //Insecure Cryptographic Storage
        [HttpGet(Name = "EncryptData")]
        public string EncryptData(string data)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(data);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return Convert.ToBase64String(hashBytes);

            }
        }
 
        //Cross-Site Scripting (XSS)
        [HttpGet(Name = "GetUserInput")]
        public string GetUserInput(string userInput)
        {
            // Simulate returning user input without sanitization
            return $"<html><body>{userInput}</body></html>";
        }

        //Insecure Deserialization
        [HttpGet(Name = "DeserializeData")]
        public object DeserializeData(string serializedData)
        {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
            BinaryFormatter formatter = new BinaryFormatter();
#pragma warning restore SYSLIB0011 // Type or member is obsolete
            using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(serializedData)))
            {
                return formatter.Deserialize(stream);
            }
        }

        //Command Injection
        [HttpGet(Name = "ExecuteCommand")]
        public void ExecuteCommand(string userInput)
        {
            string command = $"cmd.exe /c {userInput}";
            System.Diagnostics.Process.Start(command);
        }

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
