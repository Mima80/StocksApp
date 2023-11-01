using Microsoft.AspNetCore.Mvc;
using StocksApp.Models;
using ServiceContracts;
using ServiceContracts.DTO;
using StocksApp.ViewModels;

namespace StocksApp.Controllers
{
    [Route("[controller]")]
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
        [Route("[action]")]
        [Route("~/[controller]")]
        [Route("/{stockSymbolFromUser}")]
        public async Task<IActionResult> Index([FromRoute] string? stockSymbolFromUser)
        {
            stockSymbolFromUser = stockSymbolFromUser == "favicon.ico" ? null : stockSymbolFromUser;
            var stockSymbol = stockSymbolFromUser ?? _configuration["TradingOptions:DefaultStockSymbol"];
            var companyProfileDictionary = _finnhubService.GetCompanyProfile(stockSymbol).Result;
            var stockQuoteDictionary = _finnhubService.GetStockPriceQuote(stockSymbol).Result;

            var stockTrade = new StockTrade
            {
                StockSymbol = companyProfileDictionary["ticker"].ToString(),
                StockName = companyProfileDictionary["name"].ToString(),
                Price = double.Parse(stockQuoteDictionary["c"].ToString().Replace('.', ','))
            };
            return View(stockTrade);
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest buyOrderRequest)
        {
            buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;
            BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(buyOrderRequest);
            return RedirectToAction(nameof(Orders));
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SellOrder(SellOrderRequest sellOrderRequest)
        {
            sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;
            SellOrderResponse sellOrderResponse = await _stocksService.CreateSellOrder(sellOrderRequest);
            return RedirectToAction(nameof(Orders));
        }

        [Route("[action]")]
        public async Task<IActionResult> Orders()
        {
            var Orders = new Orders
            {
                BuyOrders = await _stocksService.GetBuyOrders(),
                SellOrders = await _stocksService.GetSellOrders()
            };
            return View(Orders);
        }
    }
}
