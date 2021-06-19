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
                var stock_info = _stockService.RequestStockInfo(stock_code);
                return new Models.MessageProcessorResult() { Ok = true, CustomMessage = $"{stock_code.ToUpper()} quote  is ${stock_info.Close:N2}" };
            }
            else
            {
                var messageAdded = await _dataAccess.AddMessage(message);
                return new Models.MessageProcessorResult() { Ok = true };
            }
        }
    }
}
