using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using Xunit;

namespace api.Tests
{
    public class CurrencyExchangeControllerTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly CurrencyExchangeController _controller;

        public CurrencyExchangeControllerTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _controller = new CurrencyExchangeController(_httpClient);
        }

        [Fact]
        public async Task GetExchangeRate_ReturnsExpectedExchangeRate()
        {
            // Arrange
            var baseCurrency = "USD";
            var targetCurrency = "EUR";
            var expectedResponse = "{\"rates\":{\"EUR\":0.85},\"base\":\"USD\",\"date\":\"2021-09-01\"}";

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(expectedResponse)
                });

            // Act
            var result = await _controller.GetExchangeRate(baseCurrency, targetCurrency) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(expectedResponse, result.Value);
        }

        [Fact]
        public async Task GetHistoricalExchangeRates_ReturnsExpectedHistoricalExchangeRates()
        {
            // Arrange
            var baseCurrency = "USD";
            var targetCurrency = "EUR";
            var startDate = "2021-08-01";
            var endDate = "2021-09-01";
            var expectedResponse = "{\"rates\":{\"2021-08-01\":{\"EUR\":0.84},\"2021-09-01\":{\"EUR\":0.85}},\"base\":\"USD\"}";

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(expectedResponse)
                });

            // Act
            var result = await _controller.GetHistoricalExchangeRates(baseCurrency, targetCurrency, startDate, endDate) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(expectedResponse, result.Value);
        }

        [Fact]
        public async Task GetExchangeRate_ReturnsBadRequestOnError()
        {
            // Arrange
            var baseCurrency = "USD";
            var targetCurrency = "EUR";

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest
                });

            // Act
            var result = await _controller.GetExchangeRate(baseCurrency, targetCurrency) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Error retrieving exchange rate.", result.Value);
        }

        [Fact]
        public async Task GetHistoricalExchangeRates_ReturnsBadRequestOnError()
        {
            // Arrange
            var baseCurrency = "USD";
            var targetCurrency = "EUR";
            var startDate = "2021-08-01";
            var endDate = "2021-09-01";

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest
                });

            // Act
            var result = await _controller.GetHistoricalExchangeRates(baseCurrency, targetCurrency, startDate, endDate) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Error retrieving historical exchange rates.", result.Value);
        }
    }
}
