using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using RepositoryContracts;

namespace Repositories
{
    public class FinnhubRepository : IFinnhubRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        private const string CacheKey = "StocksCacheKey";

        public FinnhubRepository (IHttpClientFactory httpClientFactory, IConfiguration configuration, IMemoryCache memoryCache)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _memoryCache = memoryCache;
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
            if (_memoryCache.TryGetValue(CacheKey, out List<Dictionary<string, string>>? responseList))
            {
                return responseList;
            }
            var httpClient = _httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/stock/symbol?exchange=US&token={_configuration["FinnhubToken"]}")
            };
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            responseList = await httpResponseMessage.Content.ReadFromJsonAsync<List<Dictionary<string, string>>?>();
            
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

            var memoryCacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                .SetPriority(CacheItemPriority.Normal);
            _memoryCache.Set(CacheKey, responseList, memoryCacheOptions);

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
