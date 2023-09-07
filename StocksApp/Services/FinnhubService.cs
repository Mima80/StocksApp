using System.Text.Json;
using StocksApp.ServiceContracts;
namespace StocksApp.Services
{
    public class FinnhubService : IFinnhubService
    {
        //GetCompanyProfile: https://finnhub.io/api/v1/stock/profile2?symbol={symbol}&token={token}
        //GetStockPriceQuote: https://finnhub.io/api/v1/quote?symbol={symbol}&token={token}
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        public FinnhubService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}")
            };
            var httpResponseMessage = httpClient.Send(httpRequestMessage);
            var responseDictionary = httpResponseMessage.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            if (responseDictionary.Result == null)
                throw new InvalidOperationException("No response from server");
            if (responseDictionary.Result.ContainsKey("error"))
                throw new InvalidOperationException(Convert.ToString(responseDictionary.Result["error"]));
            return responseDictionary;
        }
        public Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}")
            };
            var httpResponseMessage = httpClient.Send(httpRequestMessage);
            var responseDictionary = httpResponseMessage.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            if (responseDictionary.Result == null)
                throw new InvalidOperationException("No response from server");
            if (responseDictionary.Result.ContainsKey("error"))
                throw new InvalidOperationException(Convert.ToString(responseDictionary.Result["error"]));
            return responseDictionary;
        }
    }
}