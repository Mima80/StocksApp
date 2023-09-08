using System.ComponentModel.DataAnnotations;
using Entities;


namespace ServiceContracts.DTO
{
    public class BuyOrderRequest
    {
        public string? StockSymbol { get; set; }
        [Required]
        public string? StockName { get; set; }
        public DateTime? DateAndTimeOfOrder { get; set; }
        [Range(1, 100000)]
        public uint? Quantity { get; set; }
        [Range(1, 100000)]
        public double? Price { get; set; }

        public BuyOrder ToBuyOrder()
        {
            return new BuyOrder
            {
                StockSymbol = StockSymbol,
                StockName = StockName,
                DateAndTimeOfOrder = DateAndTimeOfOrder,
                Quantity = Quantity,
                Price = Price
            };
        }
    }
}
