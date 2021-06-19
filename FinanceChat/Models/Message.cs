using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceChat.Models
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
        public virtual ChatUser Sender { get; set; }
    }
}
