namespace StocksApp.Models
{
    public class StockTrade
    {
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public double Price { get; set; }
        public string? Logo { get; set; } = string.Empty;
        public string? Industry { get; set; } = string.Empty;
        public string? Exchange { get; set; } = string.Empty;
        public uint Quantity { get; set; }
    }
}
