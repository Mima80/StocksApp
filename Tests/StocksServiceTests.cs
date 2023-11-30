using Entities;
using Moq;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace Tests
{
    public class StocksServiceTests
    {
        private readonly IStocksService _stocksService;
        private readonly Mock<IStocksRepository> _stocksRepositoryMock;
        private readonly IStocksRepository _stocksRepository;

        public StocksServiceTests()
        {
            _stocksRepositoryMock = new Mock<IStocksRepository>();
            _stocksRepository = _stocksRepositoryMock.Object;
            _stocksService = new StocksService(_stocksRepository);
        }
        #region CreateBuyOrder

        [Fact]
        public async Task CreateBuyOrder_NullInput()
        {
            //arrange
            BuyOrderRequest buyOrderToCreate = null;

            //act
            await Assert.ThrowsAsync<ArgumentNullException>(() => _stocksService.CreateBuyOrder(buyOrderToCreate));
        }

        [Fact]
        public async Task CreateBuyOrder_IncorrectOrderDate()
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

            _stocksRepositoryMock.Setup(repo => repo.CreateBuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(new BuyOrder { BuyOrderID = Guid.NewGuid() });

            //act
            await Assert.ThrowsAsync<ArgumentException>(() => _stocksService.CreateBuyOrder(buyOrderToCreate));
        }

        [Fact]
        public async Task CreateBuyOrder_PriceBelowBounds()
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
        public async Task CreateBuyOrder_PriceAboveBounds()
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
        public async Task CreateBuyOrder_QuantityAboveBounds()
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
        public async Task CreateBuyOrder_CorrectData()
        {
            //arrange
            var buyOrderToCreate = new BuyOrderRequest
            {
                DateAndTimeOfOrder = DateTime.Parse("2021-12-21"),
                Price = 10,
                Quantity = 10,
                StockName = "MSFT",
                StockSymbol = "MSFT"
            };

            _stocksRepositoryMock.Setup(repo => repo.CreateBuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(new BuyOrder { BuyOrderID = Guid.NewGuid() });
            //act
            var buyOrderResponse = await _stocksService.CreateBuyOrder(buyOrderToCreate);
            //assert
            Assert.NotEqual(Guid.Empty, buyOrderResponse.BuyOrderID);
        }
        #endregion

        #region CreateSellOrder
        [Fact]
        public async Task CreateSellOrder_NullInput()
        {
            //arrange
            SellOrderRequest sellOrderRequest = null;
            //act
            await Assert.ThrowsAsync<ArgumentNullException>(() => _stocksService.CreateSellOrder(sellOrderRequest));
        }

        [Fact]
        public async Task CreateSellOrder_PriceBelowBounds()
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
        public async Task CreateSellOrder_PriceAboveBounds()
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
        public async Task CreateSellOrder_QuantityAboveBounds()
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
        public async Task CreateSellOrder_IncorrectDateAndTimeOfOrder()
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
        public async Task CreateSellOrder_CorrectData()
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

            _stocksRepositoryMock.Setup(repo => repo.CreateSellOrder(It.IsAny<SellOrder>()))
                .ReturnsAsync(new SellOrder() { SellOrderID = Guid.NewGuid() });
            //act
            var sellOrderResponse = await _stocksService.CreateSellOrder(sellOrderRequest);

            //assert
            Assert.NotEqual(Guid.Empty, sellOrderResponse.SellOrderID);
        }
        #endregion

        #region GetBuyOrders

        [Fact]
        public async Task GetBuyOrders_DefaultList()
        {
            _stocksRepositoryMock.Setup(repo => repo.GetBuyOrders())
                .ReturnsAsync(new List<BuyOrder>());
            //assert
            Assert.Empty( await _stocksService.GetBuyOrders());
        }

        [Fact]
        public async Task GetBuyOrders_AddingSomeOrders()
        {
            var listOfOrderRequests = new List<BuyOrderRequest>
            {
                new() { DateAndTimeOfOrder = DateTime.Parse("2021-12-12"), Price = 12, Quantity = 12, StockName = "blanc", StockSymbol = "blanc" },
                new() { DateAndTimeOfOrder = DateTime.Parse("2021-12-12"), Price = 13, Quantity = 12, StockName = "blanc2", StockSymbol = "blanc2" },
                new() { DateAndTimeOfOrder = DateTime.Parse("2021-12-12"), Price = 14, Quantity = 12, StockName = "blanc3", StockSymbol = "blanc3" }
            };
            var listOfOrderResponsesFromAdd = new List<BuyOrderResponse>();

            foreach (var buyOrderRequest in listOfOrderRequests)
            {
                _stocksRepositoryMock.Setup(repo => repo.CreateBuyOrder(It.IsAny<BuyOrder>()))
                    .ReturnsAsync(buyOrderRequest.ToBuyOrder());
                listOfOrderResponsesFromAdd.Add(await _stocksService.CreateBuyOrder(buyOrderRequest));
            }

            _stocksRepositoryMock.Setup(repo => repo.GetBuyOrders())
                .ReturnsAsync(listOfOrderRequests.Select(p => p.ToBuyOrder()).ToList());

            var listOfOrderResponsesFromGet = await _stocksService.GetBuyOrders();
            //assert
            foreach (var buyOrderResponse in listOfOrderResponsesFromAdd)
            {
                Assert.Contains(buyOrderResponse, listOfOrderResponsesFromGet);
            }
        }
        #endregion

        #region GetSellOrders

        [Fact]
        public async Task GetSellOrders_DefaultList()
        {
            _stocksRepositoryMock.Setup(repo => repo.GetSellOrders())
                .ReturnsAsync(new List<SellOrder>());
            //assert
            Assert.Empty(await _stocksService.GetSellOrders());
        }

        [Fact]
        public async Task GetSellOrders_AddingSomeOrders()
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
                _stocksRepositoryMock.Setup(repo => repo.CreateSellOrder(It.IsAny<SellOrder>()))
                    .ReturnsAsync(sellOrderRequest.ToSellOrder());
                listOfOrderResponsesFromAdd.Add(await _stocksService.CreateSellOrder(sellOrderRequest));
            }

            _stocksRepositoryMock.Setup(repo => repo.GetSellOrders())
                .ReturnsAsync(listOfOrderRequests.Select(p => p.ToSellOrder()).ToList());

            var listOfOrderResponsesFromGet = await _stocksService.GetSellOrders();
            //assert
            foreach (var sellOrderResponse in listOfOrderResponsesFromAdd)
            {
                Assert.Contains(sellOrderResponse, listOfOrderResponsesFromGet);
            }
        }
        #endregion
    }
}