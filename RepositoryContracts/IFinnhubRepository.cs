namespace RepositoryContracts
{
    public interface IFinnhubRepository
    {
        Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);
        Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);
        Task<List<Dictionary<string, string>>?> GetStocks();
        Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch);
        //FinnhubRepository:
        //Create a Repository interface called 'IFinnhubRepository' with following methods:
        //Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);
        //Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);
        //Task<List<Dictionary<string, string>>?> GetStocks();
        //Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch);
        //Implement the above Repository interface called 'IFinnhubRepository' that sends request to the respective url and returns its response as a Dictionary<string, object>.
        //GetCompanyProfile: https://finnhub.io/api/v1/stock/profile2?symbol={symbol}&token={token}
        //GetStockPriceQuote: https://finnhub.io/api/v1/quote?symbol={symbol}&token={token}
        //GetStocks: https://finnhub.io/api/v1/stock/symbol?exchange=US&token={token}
        //SearchStocks: https://finnhub.io/api/v1/search?q={stockNameToSearch}&token={token}
    }
}