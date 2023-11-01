using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class StocksService : IStocksService
    {
        private readonly OrderDbContext _orderDbContext;

        public StocksService(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }
        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if (buyOrderRequest == null)
                throw new ArgumentNullException();
            ValidationHelper.ModelValidation(buyOrderRequest);
            var buyOrderToAdd = buyOrderRequest.ToBuyOrder();
            buyOrderToAdd.BuyOrderID = Guid.NewGuid();
            await _orderDbContext.BuyOrders.AddAsync(buyOrderToAdd);
            await _orderDbContext.SaveChangesAsync();
            return buyOrderToAdd.ToBuyOrderResponse();
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null)
                throw new ArgumentNullException();
            ValidationHelper.ModelValidation(sellOrderRequest);
            var sellOrderToAdd = sellOrderRequest.ToSellOrder();
            sellOrderToAdd.SellOrderID = Guid.NewGuid();
            await _orderDbContext.SellOrders.AddAsync(sellOrderToAdd);
            await _orderDbContext.SaveChangesAsync();
            return sellOrderToAdd.ToSellOrderResponse();
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            var buyOrderResponses = await _orderDbContext.BuyOrders.Select(buyOrder => buyOrder.ToBuyOrderResponse()).ToListAsync();
            return new List<BuyOrderResponse>(buyOrderResponses.OrderBy(p => p.DateAndTimeOfOrder).Reverse());
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            var sellOrderResponses = await _orderDbContext.SellOrders.Select(sellOrder => sellOrder.ToSellOrderResponse()).ToListAsync();
            return new List<SellOrderResponse>(sellOrderResponses.OrderBy(p => p.DateAndTimeOfOrder).Reverse());
        }
    }
}
