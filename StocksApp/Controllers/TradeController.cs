using Microsoft.AspNetCore.Mvc;
using StocksApp.Models;
using ServiceContracts;
using ServiceContracts.DTO;
using StocksApp.ViewModels;

namespace StocksApp.Controllers
{
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
        [Route("/{stockSymbolFromUser}")]
        public IActionResult Index([FromRoute] string? stockSymbolFromUser, bool buyPressed, bool sellPressed, uint quantity)
        {
            var stockSymbol = stockSymbolFromUser ?? _configuration["TradingOptions:DefaultStockSymbol"];
            var companyProfileDictionary = _finnhubService.GetCompanyProfile(stockSymbol).Result;
            var stockQuoteDictionary = _finnhubService.GetStockPriceQuote(stockSymbol).Result;

            if (quantity != 0)
            {
                if (buyPressed)
                {
                    _stocksService.CreateBuyOrder(new BuyOrderRequest
                    {
                        DateAndTimeOfOrder = DateTime.Now,
                        Price = double.Parse(stockQuoteDictionary["c"].ToString().Replace('.', ',')),
                        Quantity = quantity,
                        StockName = companyProfileDictionary["name"].ToString(),
                        StockSymbol = stockSymbol
                    });
                }
                if (sellPressed)
                {
                    _stocksService.CreateSellOrder(new SellOrderRequest()
                    {
                        DateAndTimeOfOrder = DateTime.Now,
                        Price = double.Parse(stockQuoteDictionary["c"].ToString().Replace('.', ',')),
                        Quantity = quantity,
                        StockName = companyProfileDictionary["name"].ToString(),
                        StockSymbol = stockSymbol
                    });
                }
            }

            var stockTrade = new StockTrade
            {
                StockSymbol = companyProfileDictionary["ticker"].ToString(),
                StockName = companyProfileDictionary["name"].ToString(),
                Price = double.Parse(stockQuoteDictionary["c"].ToString().Replace('.', ','))
            };
            return View(stockTrade);
        }

        [Route("orders")]
        public IActionResult Orders()
        {
            var Orders = new Orders
            {
                BuyOrders = _stocksService.GetBuyOrders().Result,
                SellOrders = _stocksService.GetSellOrders().Result
            };
            return View(Orders);
        }
    }
}
