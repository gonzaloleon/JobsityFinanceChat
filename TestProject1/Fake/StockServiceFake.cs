using StockBot.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProject1.Fake
{
    public class StockServiceFake : IStockService
    {
        private readonly List<FinanceCommon.Models.StockModel> _mockedStocks;
        public StockServiceFake()
        {
            _mockedStocks = new List<FinanceCommon.Models.StockModel>() {
            new FinanceCommon.Models.StockModel(){ Close = 1234, Date = new DateTime(2021,6,19), High = 100, Low = 98,
            Open = 99, Symbol = "mocked1", Time = "11:59:05", Volume = 1321323 }
            };
        }
        public FinanceCommon.Models.StockRequestResponse GetStock(string stock_code)
        {
            var def = new FinanceCommon.Models.StockModel()
            {
                Close = default,
                Date = default,
                High = default,
                Low = default,
                Open = default,
                Symbol = stock_code,
                Time = default,
                Volume = default,
                NotListed = true,
            };
            return new FinanceCommon.Models.StockRequestResponse()
            {
                Ok = true,
                StockInfo = _mockedStocks.Any(r => r.Symbol == stock_code) ? _mockedStocks.First(r => r.Symbol == stock_code) :
                    def,
                ErrorMessage = !_mockedStocks.Any(r => r.Symbol == stock_code) ? $"Stock Code {stock_code} is not listed." : "",
            };
        }
    }
}
