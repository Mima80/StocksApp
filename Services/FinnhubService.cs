using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using ServiceContracts;

namespace Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        public FinnhubService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}")
            };
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            var responseDictionary = await httpResponseMessage.Content.ReadFromJsonAsync<Dictionary<string, object>>();

            if (responseDictionary == null)
                throw new InvalidOperationException("No response from server");
            if (responseDictionary.ContainsKey("error"))
                throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));

            return responseDictionary;
        }
        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}")
            };
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            var responseDictionary = await httpResponseMessage.Content.ReadFromJsonAsync<Dictionary<string, object>>();

            if (responseDictionary == null)
                throw new InvalidOperationException("No response from server");
            if (responseDictionary.ContainsKey("error"))
                throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));

            return responseDictionary;
        }
    }
}