using Microsoft.AspNetCore.Mvc;
using StockBot.Controllers;
using StockBot.Service;
using FluentAssertions;
using Xunit;

namespace TestProject1
{
    public class StockBotServiceTest
    {
        private readonly IStockService _stockBotService;
        private readonly StockController _stockController;
        public StockBotServiceTest()
        {
            _stockBotService = new Fake.StockServiceFake();
            _stockController = new StockController(_stockBotService);
        }
        [Fact]
        public void TestInvalidStockCode()
        {
            var result = _stockController.Get("stock_stock");
            Assert.NotNull(result);
            Assert.IsAssignableFrom<ActionResult<FinanceCommon.Models.StockModel>>(result);
            var okResult = (OkObjectResult)result.Result;
            Assert.IsType<FinanceCommon.Models.StockModel>(okResult.Value);
            var def = new FinanceCommon.Models.StockModel()
            {
                Close = default,
                Date = default,
                High = default,
                Low = default,
                Open = default,
                Symbol = "stock_stock",
                Time = default,
                Volume = default
            };
            var okResultParsed = (FinanceCommon.Models.StockModel)okResult.Value;
            okResultParsed.Should().BeEquivalentTo(def);
        }

        [Fact]
        public void TestValidStockCode()
        {
            var result = _stockController.Get("mocked1");
            Assert.NotNull(result);
            Assert.IsAssignableFrom<ActionResult<FinanceCommon.Models.StockModel>>(result);
            var okResult = (OkObjectResult)result.Result;
            Assert.IsType<FinanceCommon.Models.StockModel>(okResult.Value);
            var def = new FinanceCommon.Models.StockModel()
            {
                Close = default,
                Date = default,
                High = default,
                Low = default,
                Open = default,
                Symbol = "mocked1",
                Time = default,
                Volume = default
            };
            var okResultParsed = (FinanceCommon.Models.StockModel)okResult.Value;
            okResultParsed.Should().NotBeEquivalentTo(def);
        }
    }
}
