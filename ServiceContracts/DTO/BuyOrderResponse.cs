using System.ComponentModel.DataAnnotations;
using Entities;

namespace ServiceContracts.DTO
{
    public class BuyOrderResponse
    {
        public Guid? BuyOrderID { get; set; }
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }
        public DateTime? DateAndTimeOfOrder { get; set; }
        public uint? Quantity { get; set; }
        public double? Price { get; set; }
        public double? TradeAmount { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            var other = obj as BuyOrderResponse;
            return DateAndTimeOfOrder == other.DateAndTimeOfOrder 
                   && StockSymbol == other.StockSymbol 
                   && StockName == other.StockName 
                   && Price == other.Price 
                   && TradeAmount == other.TradeAmount;
        }
    }

    public static class BuyOrderExtensions
    {
        public static BuyOrderResponse ToBuyOrderResponse(this BuyOrder buyOrder)
        {
            return new BuyOrderResponse
            {
                BuyOrderID = buyOrder.BuyOrderID,
                StockSymbol = buyOrder.StockSymbol,
                StockName = buyOrder.StockName,
                DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder,
                Price = buyOrder.Price,
                Quantity = buyOrder.Quantity,
                TradeAmount = buyOrder.Price * buyOrder.Quantity
            };
        }
    }
}
