using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace FinanceChat.Models
{
    public class ChatUser : IdentityUser
    {
        public ChatUser()
        {
            Messages = new List<Message>();
        }
        public virtual List<Message> Messages { get; set; }
    }
}
