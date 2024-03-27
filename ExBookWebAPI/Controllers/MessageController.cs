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
            int user_id = (int)HttpContext.Items["User_id"];
            message.sender_id = user_id;
            db.Messages.Add(message);
            db.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("NotificationCheck")]
        public async Task<ActionResult<IEnumerable<Message>>> NotificationCheck()
        {

            int user_id = (int)HttpContext.Items["User_id"];
            var messages = await db.Messages
                .Where(x => x.sender_id == user_id || x.recipient_id == user_id)
                .ToListAsync();
            return messages;
        }
    }
}
