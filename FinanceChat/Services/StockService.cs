using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FinanceChat.Services
{
    public interface IStockService
    {
        Task<FinanceCommon.Models.StockRequestResponse> RequestStockInfoAsync(string stock_code);
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

        public async Task<FinanceCommon.Models.StockRequestResponse> RequestStockInfoAsync(string stock_code)
        {
            try
            {
                using (HttpResponseMessage response = await _httpClient.GetAsync($"{_stockBotServiceAPIUrl}?stock_code={stock_code}"))
                using (HttpContent content = response.Content)
                {
                    string serviceResponse = await content.ReadAsStringAsync();
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        return new FinanceCommon.Models.StockRequestResponse()
                        {
                            Ok = false,
                            ErrorMessage = "API Server response error.",
                            StockInfo = null,
                        };
                    }
                    else
                    {
                        var stock = JsonConvert.DeserializeObject<FinanceCommon.Models.StockRequestResponse>(serviceResponse);
                        return stock;
                    }
                }
            }
            catch
            {
                return new FinanceCommon.Models.StockRequestResponse() {
                    Ok = false, ErrorMessage = "API request error.", StockInfo = null,
                };
            }
        }
    }
}
