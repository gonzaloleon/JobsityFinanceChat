namespace FinanceChat.Models
{
    public class MessageProcessorResult
    {
        public bool Ok { get; set; }
        public string CustomMessage { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
