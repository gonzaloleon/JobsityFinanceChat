using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialChat.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string SenderID { get; set; }
        public string Username { get; set; }
        [Required]
        public string Text { get; set; }
        public DateTime MessageTime { get; set; }
        [Required] 
        public virtual User Sender { get; set; }
    }
}
