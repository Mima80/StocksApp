using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace Tests
{
    public class StocksServiceTests
    {
        private readonly StocksService _stocksService;
        public StocksServiceTests()
        {
            _stocksService = new StocksService();
        }
        [Fact]
        public async void CreateBuyOrder_NullInput()
        {
            //arrange
            BuyOrderRequest buyOrderToCreate = null;
            //act
            await Assert.ThrowsAsync<ArgumentNullException>(() => _stocksService.CreateBuyOrder(buyOrderToCreate));
        }
    }
}