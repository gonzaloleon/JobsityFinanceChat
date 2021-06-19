using System.Threading.Tasks;

namespace FinanceChat.Services
{
    public interface IMessageProcessorService
    {
        Task<Models.MessageProcessorResult> ProcessMessage(Models.Message message);
    }
    public class MessageProcessorService : IMessageProcessorService
    {
        private readonly Back.IDataAccess _dataAccess;
        private readonly Services.IStockService _stockService;
        public MessageProcessorService(Back.IDataAccess dataAccess, Services.IStockService stockService)
        {
            _dataAccess = dataAccess;
            _stockService = stockService;
        }

        public async Task<Models.MessageProcessorResult> ProcessMessage(Models.Message message)
        {
            if (message.Text.StartsWith("/stock="))
            {
                var mTextSplitted = message.Text.Split("=");
                var stock_code = mTextSplitted[1];
                if (!string.IsNullOrEmpty(stock_code))
                {
                    var stock_info = _stockService.RequestStockInfo(stock_code);
                    if (stock_info.Ok)
                    {
                        return new Models.MessageProcessorResult()
                        {
                            Ok = true,
                            CustomMessage = stock_info.StockInfo.NotListed ? stock_info.ErrorMessage : 
                            $"{stock_code.ToUpper()} quote is $ {(stock_info.StockInfo.Close):N2}"
                        };
                    }
                    else
                    {
                        return new Models.MessageProcessorResult() { Ok = false, CustomMessage = $"{stock_info.ErrorMessage}" };
                    }
                }
                else
                {
                    return new Models.MessageProcessorResult() { Ok = true, CustomMessage = "You need to input a STOCK_CODE to get quote, e.g.: aapl.us" };
                }
            }
            else
            {
                var messageAdded = await _dataAccess.AddMessage(message);
                return new Models.MessageProcessorResult() { Ok = messageAdded };
            }
        }
    }
}
