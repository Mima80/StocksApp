using Microsoft.AspNetCore.Mvc;
using StocksApp.Models;
using StocksApp.ServiceContracts;
using System.Diagnostics;

namespace StocksApp.Controllers
{
    public class TradeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IConfiguration _configuration;

        public TradeController(IFinnhubService finnhubService, IConfiguration configuration)
        {
            _finnhubService = finnhubService;
            _configuration = configuration;
        }
        [Route("/")]
        public IActionResult Index()
        {
            var companyProfileDictionary = _finnhubService.GetCompanyProfile(_configuration["TradingOptions:DefaultStockSymbol"]).Result;
            var stockQuoteDictionary = _finnhubService.GetStockPriceQuote(_configuration["TradingOptions:DefaultStockSymbol"]).Result;
            var stockTrade = new StockTrade()
            {
                StockSymbol = companyProfileDictionary["ticker"].ToString(),
                StockName = companyProfileDictionary["name"].ToString(),
                Price = double.Parse(stockQuoteDictionary["c"].ToString().Replace('.', ','))
            };
            return View(stockTrade);
        }
    }
}
