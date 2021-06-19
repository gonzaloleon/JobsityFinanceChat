using FinanceChat.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace FinanceChat.Pages
{
    public class IndexModel : PageModel
    {
        private readonly Back.IDataAccess _dataAccess;
        private readonly Services.IMessageProcessorService _messageProcessorService;
        private readonly UserManager<Models.ChatUser> _userManager;
        private readonly ILogger<IndexModel> _logger;
        private readonly IHubContext<Hubs.Chat> _hubContext;
        public List<Models.Message> messages;
        [BindProperty]
        public string MessageText { get; set; }
        public IndexModel(Back.IDataAccess dataAccess, Services.IMessageProcessorService messageProcessorService,
            UserManager<Models.ChatUser> userManager, IHubContext<Hubs.Chat> hubContext, 
            ILogger<IndexModel> logger)
        {
            _dataAccess = dataAccess;
            _messageProcessorService = messageProcessorService;
            _logger = logger;
            _userManager = userManager;
            _hubContext = hubContext;
        }
        public async Task OnGet()
        {
            messages = await _dataAccess.GetLastMessages();
            messages = messages.OrderByDescending(r => r.MessageTime)
                .Take(50)
                .OrderBy(r=>r.MessageTime)
                .ToList();
        }

        public async Task<IActionResult> OnPostSendMessage(string messageText)
        {
            var sender = await _userManager.GetUserAsync(User);
            Models.Message msg = new Models.Message()
            {
                Username = User.Identity.Name,
                Text = messageText,
                MessageTime = DateTime.Now,
                Sender = sender,
                SenderID = sender.Id,
            };
            var mpresult = await _messageProcessorService.ProcessMessage(msg);
            if (mpresult.Ok)
            {
                //await new Hubs.Chat().SendMessage(msg.Text);
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", !string.IsNullOrEmpty(mpresult.CustomMessage) ? "Server" :
                    User.Identity.Name, !string.IsNullOrEmpty(mpresult.CustomMessage) ? mpresult.CustomMessage : msg.Text);
            }
            return new JsonResult(mpresult);
        }
    }
}
