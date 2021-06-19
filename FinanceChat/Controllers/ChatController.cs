using FinanceChat.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceChat.Controllers
{
    public class ChatController : Controller
    {
        public readonly ApplicationDbContext _context;
        public readonly UserManager<Models.ChatUser> _userManager;

        public ChatController(ApplicationDbContext context, UserManager<Models.ChatUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<ActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            ViewBag.CurrentUsername = currentUser.UserName;
            var messages = _context.Messages.ToList();
            return View();
        }

        public async Task<IActionResult> Create(Models.Message message)
        {
            if (ModelState.IsValid)
            {
                message.Username = User.Identity.Name;
                var sender = await _userManager.GetUserAsync(User);
                message.Sender = sender;
                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NoContent();
        }
    }
}
