using System;

namespace FinanceCommon
{
    public class Models
    {
        public class StockRequestResponse
        {
            public bool Ok { get; set; }
            public StockModel StockInfo { get; set; }
            public string ErrorMessage { get; set; }
        }
        public class StockModel
        {
            public string Symbol { get; set; }
            public DateTime Date { get; set; }
            public string Time { get; set; }
            public double Open { get; set; }
            public double High { get; set; }
            public double Low { get; set; }
            public double Close { get; set; }
            public double Volume { get; set; }
            public bool NotListed { get; set; } = false;
        }
    }
}
