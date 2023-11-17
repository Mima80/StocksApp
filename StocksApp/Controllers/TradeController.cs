using Microsoft.AspNetCore.Mvc;
using StocksApp.Models;
using ServiceContracts;
using ServiceContracts.DTO;
using StocksApp.ViewModels;

namespace StocksApp.Controllers
{
    [Route("[controller]/[action]")]
    public class TradeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IStocksService _stocksService;
        private readonly IConfiguration _configuration;

        public TradeController(IFinnhubService finnhubService, IConfiguration configuration, IStocksService stocksService)
        {
            _finnhubService = finnhubService;
            _configuration = configuration;
            _stocksService = stocksService;
        }

        [Route("/")]
        [Route("{stockSymbolFromUser}")]
        public async Task<IActionResult> Index([FromRoute] string? stockSymbolFromUser)
        {
            stockSymbolFromUser = stockSymbolFromUser == "favicon.ico" ? null : stockSymbolFromUser;
            var stockSymbol = stockSymbolFromUser ?? _configuration["TradingOptions:DefaultStockSymbol"];
            var companyProfileDictionary = await _finnhubService.GetCompanyProfile(stockSymbol);
            var stockQuoteDictionary = await _finnhubService.GetStockPriceQuote(stockSymbol);
            ViewBag.FinnhubToken = _configuration["FinnhubToken"] ?? throw new KeyNotFoundException();
            var stockTrade = new StockTrade
            {
                StockSymbol = companyProfileDictionary["ticker"].ToString(),
                StockName = companyProfileDictionary["name"].ToString(),
                Price = double.Parse(stockQuoteDictionary["c"].ToString().Replace('.', ','))
            };
            return View(stockTrade);
        }


        [HttpPost]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest buyOrderRequest)
        {
            buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;
            await _stocksService.CreateBuyOrder(buyOrderRequest);
            return RedirectToAction(nameof(Orders));
        }

        [HttpPost]
        public async Task<IActionResult> SellOrder(SellOrderRequest sellOrderRequest)
        {
            sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;
            await _stocksService.CreateSellOrder(sellOrderRequest);
            return RedirectToAction(nameof(Orders));
        }

        public async Task<IActionResult> Orders()
        {
            var orders = new Orders
            {
                BuyOrders = await _stocksService.GetBuyOrders(),
                SellOrders = await _stocksService.GetSellOrders()
            };
            return View(orders);
        }
    }
}
