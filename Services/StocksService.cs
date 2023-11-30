using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services
{
    public class StocksService : IStocksService
    {
        private readonly IStocksRepository _stocksRepository;

        public StocksService(IStocksRepository stocksRepository)
        {
            _stocksRepository = stocksRepository;
        }
        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if (buyOrderRequest == null)
                throw new ArgumentNullException();
            ValidationHelper.ModelValidation(buyOrderRequest);
            var buyOrderToAdd = buyOrderRequest.ToBuyOrder();
            buyOrderToAdd.BuyOrderID = Guid.NewGuid();
            return (await _stocksRepository.CreateBuyOrder(buyOrderToAdd)).ToBuyOrderResponse();
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null)
                throw new ArgumentNullException();
            ValidationHelper.ModelValidation(sellOrderRequest);
            var sellOrderToAdd = sellOrderRequest.ToSellOrder();
            sellOrderToAdd.SellOrderID = Guid.NewGuid();
            return (await _stocksRepository.CreateSellOrder(sellOrderToAdd)).ToSellOrderResponse();
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            var buyOrders = await _stocksRepository.GetBuyOrders();
            var buyOrderResponses = buyOrders.Select(buyOrder => buyOrder.ToBuyOrderResponse());
            return new List<BuyOrderResponse>(buyOrderResponses.OrderBy(p => p.DateAndTimeOfOrder).Reverse());
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            var sellOrders = await _stocksRepository.GetSellOrders();
            var sellOrderResponses = sellOrders.Select(sellOrder => sellOrder.ToSellOrderResponse());
            return new List<SellOrderResponse>(sellOrderResponses.OrderBy(p => p.DateAndTimeOfOrder).Reverse());
        }
    }
}
