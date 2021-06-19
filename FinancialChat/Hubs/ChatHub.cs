using FinancialChat.Models;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialChat.Hubs
{
    [HubName("ChatHub")]
    public class ChatHub : Hub
    {
        public async void SendMessage(Message message)
        {
            await Clients.All.SendAsync("sendMessage", message);
        }

        //public override async Task OnConnectedAsync()
        //{
        //    await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
        //    await base.OnConnectedAsync();
        //}

        //public override async Task OnDisconnectedAsync(Exception ex)
        //{
        //    await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
        //    await base.OnDisconnectedAsync(ex);
        //}
    }
}
