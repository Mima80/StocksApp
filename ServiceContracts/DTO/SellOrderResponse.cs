using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class SellOrderResponse
    {
        public Guid SellOrderID { get; set; }
        public string StockSymbol { get; set; }
        public string StockName { get; set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        public uint Quantity { get; set; }
        public double? Price { get; set; }
        public double? TradeAmount { get; set; }
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            var other = obj as SellOrderResponse;
            return DateAndTimeOfOrder == other.DateAndTimeOfOrder
                   && StockSymbol == other.StockSymbol
                   && StockName == other.StockName
                   && Price == other.Price
                   && TradeAmount == other.TradeAmount;
        }
    }
    public static class SellOrderExtensions
    {
        public static SellOrderResponse ToSellOrderResponse(this SellOrder sellOrder)
        {
            return new SellOrderResponse
            {
                SellOrderID = sellOrder.SellOrderID,
                StockSymbol = sellOrder.StockSymbol,
                StockName = sellOrder.StockName,
                DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder,
                Price = sellOrder.Price,
                Quantity = sellOrder.Quantity,
                TradeAmount = sellOrder.Price * sellOrder.Quantity
            };
        }
    }
}
