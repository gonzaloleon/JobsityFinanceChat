using FinanceChat.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceChat.Back
{
    public interface IDataAccess
    {
        Task<bool> AddMessage(Models.Message message);
        Task<List<Models.Message>> GetLastMessages();
    }
    public class DataAccess : IDataAccess
    {
        ApplicationDbContext _context;
        public DataAccess(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddMessage(Models.Message message)
        {
            try
            {
                await _context.Messages.AddAsync(message);
                int saved = await _context.SaveChangesAsync();
                return saved > 0;
            }
            catch { return false; }
        }

        public async Task<List<Models.Message>> GetLastMessages()
        {
            var messages = await _context.Messages
                .OrderBy(r=>r.MessageTime)
                .ToListAsync()
                ;
            return messages;
        }
    }
}
