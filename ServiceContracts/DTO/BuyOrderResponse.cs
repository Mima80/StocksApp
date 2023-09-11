using System.ComponentModel.DataAnnotations;
using Entities;

namespace ServiceContracts.DTO
{
    public class BuyOrderResponse
    {
        public Guid? BuyOrderID { get; set; }
        [Required]
        public string? StockSymbol { get; set; }
        [Required]
        public string? StockName { get; set; }
        public DateTime? DateAndTimeOfOrder { get; set; }
        [Range(1, 100000)]
        public uint? Quantity { get; set; }
        [Range(1, 100000)]
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

        public override int GetHashCode()
        {
            return HashCode.Combine(BuyOrderID, StockSymbol, StockName, DateAndTimeOfOrder, Quantity, Price, TradeAmount);
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
