using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Model;
using ServiceContracts;
using StocksApp.ViewModels;
using System.Linq;

namespace StocksApp.Controllers
{
    [Route("[controller]")]
    public class StocksController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IConfiguration _configuration;

        public StocksController(IFinnhubService finnhubService, IConfiguration configuration)
        {
            _configuration = configuration;
            _finnhubService = finnhubService;
        }

        [Route("[action]")]
        public async Task<IActionResult> Explore()
        {
            var stockSymbols = _configuration["TradingOptions:Top25PopularStocks"]?.Split(",").ToList();
            var stockDictionaryList = await _finnhubService.GetStocks();
            var stocks = new List<Stock>();

            foreach (var stock in stockDictionaryList)
            {
                if (!stockSymbols.Contains(stock["symbol"])) continue;
                stocks.Add(new Stock
                {
                    StockName = stock["description"],
                    StockSymbol = stock["symbol"]
                });
            }

            return View(stocks);
        }
    }
}
