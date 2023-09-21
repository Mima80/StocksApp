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
        #region CreateBuyOrder

        [Fact]
        public async void CreateBuyOrder_NullInput()
        {
            //arrange
            BuyOrderRequest buyOrderToCreate = null;
            //act
            await Assert.ThrowsAsync<ArgumentNullException>(() => _stocksService.CreateBuyOrder(buyOrderToCreate));
        }

        [Fact]
        public async void CreateBuyOrder_IncorrectOrderDate()
        {
            //arrange
            BuyOrderRequest buyOrderToCreate = new BuyOrderRequest
            {
                DateAndTimeOfOrder = DateTime.Parse("1900-12-21"),
                Price = 100,
                Quantity = 12,
                StockName = "MSFT",
                StockSymbol = "MSFT"
            };
            //act
            await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateBuyOrder(buyOrderToCreate));
        }

        [Fact]
        public async void CreateBuyOrder_PriceBelowBounds()
        {
            //arrange
            BuyOrderRequest buyOrderToCreate = new BuyOrderRequest
            {
                DateAndTimeOfOrder = DateTime.Parse("2021-12-21"),
                Price = -1,
                Quantity = 12,
                StockName = "MSFT",
                StockSymbol = "MSFT"
            };
            //act
            await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateBuyOrder(buyOrderToCreate));
        }

        [Fact]
        public async void CreateBuyOrder_PriceAboveBounds()
        {
            //arrange
            BuyOrderRequest buyOrderToCreate = new BuyOrderRequest
            {
                DateAndTimeOfOrder = DateTime.Parse("2021-12-21"),
                Price = 100000000,
                Quantity = 12,
                StockName = "MSFT",
                StockSymbol = "MSFT"
            };
            //act
            await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateBuyOrder(buyOrderToCreate));
        }
        [Fact]
        public async void CreateBuyOrder_QuantityAboveBounds()
        {
            //arrange
            BuyOrderRequest buyOrderToCreate = new BuyOrderRequest
            {
                DateAndTimeOfOrder = DateTime.Parse("2021-12-21"),
                Price = 10,
                Quantity = 1000000000,
                StockName = "MSFT",
                StockSymbol = "MSFT"
            };
            //act
            await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateBuyOrder(buyOrderToCreate));
        }
        [Fact]
        public async void CreateBuyOrder_CorrectData()
        {
            //arrange
            BuyOrderRequest buyOrderToCreate = new BuyOrderRequest
            {
                DateAndTimeOfOrder = DateTime.Parse("2021-12-21"),
                Price = 10,
                Quantity = 10,
                StockName = "MSFT",
                StockSymbol = "MSFT"
            };
            //act
            var buyOrderResponse = await _stocksService.CreateBuyOrder(buyOrderToCreate);
            //assert
            Assert.NotEqual(Guid.Empty, buyOrderResponse.BuyOrderID);
        }
        #endregion

        #region CreateSellOrder
        [Fact]
        public async void CreateSellOrder_NullInput()
        {
            //arrange
            SellOrderRequest sellOrderRequest = null;
            //act
            await Assert.ThrowsAsync<ArgumentNullException>(() => _stocksService.CreateSellOrder(sellOrderRequest));
        }

        [Fact]
        public async void CreateSellOrder_PriceBelowBounds()
        {
            //arrange
            var sellOrderRequest = new SellOrderRequest
            {
                DateAndTimeOfOrder = DateTime.Parse("2021-12-11"),
                Price = -1,
                Quantity = 10,
                StockName = "123",
                StockSymbol = "MSFT"
            };
            //act
            await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateSellOrder(sellOrderRequest));
        }

        [Fact]
        public async void CreateSellOrder_PriceAboveBounds()
        {
            //arrange
            var sellOrderRequest = new SellOrderRequest
            {
                DateAndTimeOfOrder = DateTime.Parse("2021-12-11"),
                Price = 99999999,
                Quantity = 10,
                StockName = "123",
                StockSymbol = "MSFT"
            };
            //act
            await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateSellOrder(sellOrderRequest));
        }

        [Fact]
        public async void CreateSellOrder_QuantityAboveBounds()
        {
            //arrange
            var sellOrderRequest = new SellOrderRequest
            {
                DateAndTimeOfOrder = DateTime.Parse("2021-12-11"),
                Price = 99,
                Quantity = 1000000000,
                StockName = "123",
                StockSymbol = "MSFT"
            };
            //act
            await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateSellOrder(sellOrderRequest));
        }
        [Fact]
        public async void CreateSellOrder_IncorrectDateAndTimeOfOrder()
        {
            //arrange
            var sellOrderRequest = new SellOrderRequest
            {
                DateAndTimeOfOrder = DateTime.Parse("1900-12-11"),
                Price = 99,
                Quantity = 10,
                StockName = "123",
                StockSymbol = "MSFT"
            };
            //act
            await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateSellOrder(sellOrderRequest));
        }
        [Fact]
        public async void CreateSellOrder_CorrectData()
        {
            //arrange
            var sellOrderRequest = new SellOrderRequest
            {
                DateAndTimeOfOrder = DateTime.Parse("2021-12-11"),
                Price = 99,
                Quantity = 10,
                StockName = "123",
                StockSymbol = "MSFT"
            };
            //act
            var sellOrderResponse = await _stocksService.CreateSellOrder(sellOrderRequest);

            //assert
            Assert.NotEqual(Guid.Empty, sellOrderResponse.SellOrderID);
        }
        #endregion

        #region GetBuyOrders

        [Fact]
        public void GetBuyOrders_DefaultList()
        {
            //assert
            Assert.Empty(_stocksService.GetBuyOrders().Result);
        }

        [Fact]
        public void GetBuyOrders_AddingSomeOrders()
        {
            var listOfOrderRequests = new List<BuyOrderRequest>
            {
                new() { DateAndTimeOfOrder = DateTime.Parse("2021-12-12"), Price = 12, Quantity = 12, StockName = "blanc", StockSymbol = "blanc" },
                new() { DateAndTimeOfOrder = DateTime.Parse("2021-12-12"), Price = 12, Quantity = 12, StockName = "blanc2", StockSymbol = "blanc2" },
                new() { DateAndTimeOfOrder = DateTime.Parse("2021-12-12"), Price = 12, Quantity = 12, StockName = "blanc3", StockSymbol = "blanc3" }
            };
            var listOfOrderResponsesFromAdd = new List<BuyOrderResponse>(); 

            //act
            foreach (var buyOrderRequest in listOfOrderRequests)
            { 
                listOfOrderResponsesFromAdd.Add(_stocksService.CreateBuyOrder(buyOrderRequest).Result);
            }

            var listOfOrderResponsesFromGet = _stocksService.GetBuyOrders().Result;
            //assert
            foreach (var buyOrderResponse in listOfOrderResponsesFromAdd)
            {
              Assert.Contains(buyOrderResponse, listOfOrderResponsesFromGet);  
            }
        }
        #endregion

        #region GetSellOrders

        [Fact]
        public void GetSellOrders_DefaultList()
        {
            //assert
            Assert.Empty(_stocksService.GetSellOrders().Result);
        }

        [Fact]
        public void GetSellOrders_AddingSomeOrders()
        {
            var listOfOrderRequests = new List<SellOrderRequest>
            {
                new() { DateAndTimeOfOrder = DateTime.Parse("2021-12-12"), Price = 12, Quantity = 12, StockName = "blanc", StockSymbol = "blanc" },
                new() { DateAndTimeOfOrder = DateTime.Parse("2021-12-12"), Price = 12, Quantity = 12, StockName = "blanc2", StockSymbol = "blanc2" },
                new() { DateAndTimeOfOrder = DateTime.Parse("2021-12-12"), Price = 12, Quantity = 12, StockName = "blanc3", StockSymbol = "blanc3" }
            };
            var listOfOrderResponsesFromAdd = new List<SellOrderResponse>();

            //act
            foreach (var sellOrderRequest in listOfOrderRequests)
            {
                listOfOrderResponsesFromAdd.Add(_stocksService.CreateSellOrder(sellOrderRequest).Result);
            }

            var listOfOrderResponsesFromGet = _stocksService.GetSellOrders().Result;
            //assert
            foreach (var sellOrderResponse in listOfOrderResponsesFromAdd)
            {
                Assert.Contains(sellOrderResponse, listOfOrderResponsesFromGet);
            }
        }
        #endregion
    }
}