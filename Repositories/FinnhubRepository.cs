using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using RepositoryContracts;

namespace Repositories
{
    public class FinnhubRepository : IFinnhubRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public FinnhubRepository (IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
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

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/stock/symbol?exchange=US&token={_configuration["FinnhubToken"]}")
            };
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            var responseList = await httpResponseMessage.Content.ReadFromJsonAsync<List<Dictionary<string, string>>?>();

            if (responseList == null)
            {
                throw new InvalidOperationException("No response from server");
            }
            foreach (var responseDictionary in responseList)
            {
                if (responseDictionary == null)
                    throw new InvalidOperationException("No response from server");
                if (responseDictionary.ContainsKey("error"))
                    throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));
            }

            return responseList;
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/search?q={stockSymbolToSearch}&token={_configuration["FinnhubToken"]}")
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
