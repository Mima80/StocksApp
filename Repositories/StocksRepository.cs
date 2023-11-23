using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories
{
    public class StocksRepository : IStocksRepository
    {
        private readonly OrderDbContext _orderDbContext;

        public StocksRepository(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }
        public async Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder)
        {
            await _orderDbContext.BuyOrders.AddAsync(buyOrder);
            await _orderDbContext.SaveChangesAsync();
            return buyOrder;
        }

        public async Task<SellOrder> CreateSellOrder(SellOrder sellOrder)
        {
            await _orderDbContext.SellOrders.AddAsync(sellOrder);
            await _orderDbContext.SaveChangesAsync();
            return sellOrder;
        }

        public async Task<List<BuyOrder>> GetBuyOrders()
        {
            return await _orderDbContext.BuyOrders.ToListAsync();
        }

        public async Task<List<SellOrder>> GetSellOrders()
        {
            return await _orderDbContext.SellOrders.ToListAsync();
        }
    }
}
