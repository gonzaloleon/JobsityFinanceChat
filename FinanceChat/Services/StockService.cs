using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace FinanceChat.Services
{
    public interface IStockService
    {
        FinanceCommon.Models.StockModel RequestStockInfo(string stock_code);
    }
    public class StockService : IStockService
    {
        private readonly IConfiguration _configuration;
        private readonly string _stockBotServiceAPIUrl;
        private HttpClient _httpClient;
        public StockService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            var boturl = _configuration.GetValue<string>("StockBotURL");
            if (!string.IsNullOrEmpty(boturl))
            {
                _stockBotServiceAPIUrl = string.Concat(boturl, "/api/Stock");
            }
        }

        public FinanceCommon.Models.StockModel RequestStockInfo(string stock_code)
        {
            try
            {
                using (HttpResponseMessage response = _httpClient.GetAsync($"{_stockBotServiceAPIUrl}?stock_code={stock_code}").Result)
                using (HttpContent content = response.Content)
                {
                    string serviceResponse = content.ReadAsStringAsync().Result;
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                        return null;

                    var stock = JsonConvert.DeserializeObject<FinanceCommon.Models.StockModel>(serviceResponse);
                    return stock;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
