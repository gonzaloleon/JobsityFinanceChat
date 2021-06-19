using System;
using System.Globalization;
using System.Net.Http;

namespace StockBot.Service
{
    public interface IStockService
    {
        FinanceCommon.Models.StockRequestResponse GetStock(string stock_code);
    }
    public class StockService : IStockService
    {
        public HttpClient Client { get; }

        public StockService(HttpClient _client)
        {
            Client = _client;
        }

        /// <summary>
        /// Download & Process the CSV from the stooq server
        /// </summary>
        /// <param name="stock_code"></param>
        /// <returns>StockRequestResponse: Ok = false -> Server/Request/Response Error</returns>
        public FinanceCommon.Models.StockRequestResponse GetStock(string stock_code)
        {
            try
            {
                using (HttpResponseMessage response = Client.GetAsync($"https://stooq.com/q/l/?s={stock_code}&f=sd2t2ohlcv&h&e=csv").Result)
                using (HttpContent content = response.Content)
                {
                    var callResponse = content.ReadAsStringAsync().Result;
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        return new FinanceCommon.Models.StockRequestResponse()
                        {
                            StockInfo = null,
                            Ok = false,
                            ErrorMessage = "Error getting stock option info: SERVER RESPONSE ERROR.",
                        };
                    }
                    else
                    {
                        var data = callResponse.Substring(callResponse.IndexOf(Environment.NewLine, StringComparison.Ordinal) + 2);
                        var processedArray = data.Split(',');
                        var culture = new CultureInfo("en-US");
                        return new FinanceCommon.Models.StockRequestResponse()
                        {
                            StockInfo = new FinanceCommon.Models.StockModel()
                            {
                                Symbol = processedArray[0],
                                Date = !processedArray[1].Contains("N/D") ? Convert.ToDateTime(processedArray[1]) : default,
                                Time = !processedArray[2].Contains("N/D") ? Convert.ToDateTime(processedArray[2]).ToString("HH:mm:ss") : default,
                                Open = !processedArray[3].Contains("N/D") ? Convert.ToDouble(processedArray[3], culture) : default,
                                High = !processedArray[4].Contains("N/D") ? Convert.ToDouble(processedArray[4], culture) : default,
                                Low = !processedArray[5].Contains("N/D") ? Convert.ToDouble(processedArray[5], culture) : default,
                                Close = !processedArray[6].Contains("N/D") ? Convert.ToDouble(processedArray[6], culture) : default,
                                Volume = !processedArray[7].Contains("N/D") ? Convert.ToDouble(processedArray[7], culture) : default,
                                NotListed = processedArray[1].Contains("N/D"),
                            },
                            Ok = true,
                            ErrorMessage = processedArray[1].Contains("N/D") ? $"Stock Code {stock_code} is not listed." : "",
                        };
                    }
                }
            }
            catch
            {
                return new FinanceCommon.Models.StockRequestResponse()
                {
                    StockInfo = null,
                    Ok = false,
                    ErrorMessage = "Error getting stock option info."
                };
            }
        }
    }
}
