using ExBookWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using ExBookWebAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace ExBookWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {

        private readonly AppDbContext db;
        public MessageController(AppDbContext context)
        {
            db = context;
        }

        [HttpPost]
        [Route("SendMessage")]
        public async Task<ActionResult> SendMessage(Message message)
        {
            string? username = HttpContext.Items["Username"] as string;

            User? user = await db.Users.FirstOrDefaultAsync(x => x.username == username);
            message.sender_id = user.user_id;
            db.Messages.Add(message);
            db.SaveChanges();

            return Ok();
        }

        
    }
}
