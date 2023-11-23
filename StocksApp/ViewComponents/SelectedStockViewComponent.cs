using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using StocksApp.Models;
using StocksApp.ViewModels;

namespace StocksApp.ViewComponents
{
    public class SelectedStockViewComponent : ViewComponent
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IConfiguration _configuration;

        public SelectedStockViewComponent(IFinnhubService finnhubService, IConfiguration configuration)
        {
            _finnhubService = finnhubService;
            _configuration = configuration;
        }
        public async Task<IViewComponentResult> InvokeAsync(string stockSymbol)
        {
            var companyProfileDictionary = await _finnhubService.GetCompanyProfile(stockSymbol);
            var stockQuoteDictionary = await _finnhubService.GetStockPriceQuote(stockSymbol);
            var stockTrade = new StockTrade
            {
                StockSymbol = companyProfileDictionary["ticker"].ToString(),
                StockName = companyProfileDictionary["name"].ToString(),
                Price = double.Parse(stockQuoteDictionary["c"].ToString().Replace('.', ',')),
                Logo = companyProfileDictionary["logo"].ToString(),
                Industry = companyProfileDictionary["finnhubIndustry"].ToString(),
                Exchange = companyProfileDictionary["exchange"].ToString()
                //<span class="pill">@Model["finnhubIndustry"]</span>
                //<h3 class="mb">Exchange: <span class="pill">@Model["exchange"]</span></h3>
            };
            return View(stockTrade);
        }
    }
}
