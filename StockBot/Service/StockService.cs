using System;
using System.Net.Http;

namespace StockBot.Service
{
    public interface IStockService
    {
        FinanceCommon.Models.StockModel GetStock(string stock_code);
    }
    public class StockService : IStockService
    {
        public HttpClient Client { get; }

        public StockService(HttpClient _client)
        {
            Client = _client;
        }

        /// <summary>
        /// Download the CSV from the stooq server & generate a Stock object
        /// </summary>
        /// <param name="stock_code"></param>
        /// <returns></returns>
        public FinanceCommon.Models.StockModel GetStock(string stock_code)
        {
            using (HttpResponseMessage response = Client.GetAsync($"https://stooq.com/q/l/?s={stock_code}&f=sd2t2ohlcv&h&e=csv").Result)
            using (HttpContent content = response.Content)
            {
                var callResponse = content.ReadAsStringAsync().Result;
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new ArgumentException(callResponse);
                var data = callResponse.Substring(callResponse.IndexOf(Environment.NewLine, StringComparison.Ordinal) + 2);
                var processedArray = data.Split(',');
                return new FinanceCommon.Models.StockModel()
                {
                    Symbol = processedArray[0],
                    Date = !processedArray[1].Contains("N/D") ? Convert.ToDateTime(processedArray[1]) : default,
                    Time = !processedArray[2].Contains("N/D") ? Convert.ToDateTime(processedArray[2]).ToString("HH:mm:ss") : default,
                    Open = !processedArray[3].Contains("N/D") ? Convert.ToDouble(processedArray[3]) : default,
                    High = !processedArray[4].Contains("N/D") ? Convert.ToDouble(processedArray[4]) : default,
                    Low = !processedArray[5].Contains("N/D") ? Convert.ToDouble(processedArray[5]) : default,
                    Close = !processedArray[6].Contains("N/D") ? Convert.ToDouble(processedArray[6]) : default,
                    Volume = !processedArray[7].Contains("N/D") ? Convert.ToDouble(processedArray[7]) : default,
                };
            }
        }
    }
}
