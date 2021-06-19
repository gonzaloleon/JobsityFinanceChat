using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialChat.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            Messages = new List<Message>();
        }
        public virtual List<Message> Messages { get; set; }
    }
}
