using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace FinanceChat.Hubs
{
    public class Chat : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", Context.User.Identity.Name ?? "anonymous", message);
        }
    }
}
