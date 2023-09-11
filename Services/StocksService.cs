using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class StocksService : IStocksService
    {
        private readonly List<BuyOrder> _buyOrders = new();
        private readonly List<SellOrder> _sellOrders = new();
        public Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if (buyOrderRequest == null)
                throw new ArgumentNullException();
            ValidationHelper.ModelValidation(buyOrderRequest);
            var buyOrderToAdd = buyOrderRequest.ToBuyOrder();
            buyOrderToAdd.BuyOrderID = Guid.NewGuid();
            _buyOrders.Add(buyOrderToAdd);
            return Task.FromResult(buyOrderToAdd.ToBuyOrderResponse());
        }

        public Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null)
                throw new ArgumentNullException();
            ValidationHelper.ModelValidation(sellOrderRequest);
            var sellOrderToAdd = sellOrderRequest.ToSellOrder();
            sellOrderToAdd.SellOrderID = Guid.NewGuid();
            _sellOrders.Add(sellOrderToAdd);
            return Task.FromResult(sellOrderToAdd.ToSellOrderResponse());
        }

        public Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            var buyOrderResponses = _buyOrders.Select(buyOrder => buyOrder.ToBuyOrderResponse()).ToList();
            return Task.FromResult(buyOrderResponses);
        }

        public Task<List<SellOrderResponse>> GetSellOrders()
        {
            var sellOrderResponses = _sellOrders.Select(sellOrder => sellOrder.ToSellOrderResponse()).ToList();
            return Task.FromResult(sellOrderResponses);
        }
    }
}
