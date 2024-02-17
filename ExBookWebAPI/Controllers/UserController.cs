using Microsoft.AspNetCore.Mvc;
using ExBookWebAPI.Data;
using ExBookWebAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace Authorization.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly AppDbContext db;
        public UsersController(AppDbContext context)
        {
            db = context;
        }

        [HttpPost]
        [Route("GetUserProfile")]
        public async Task<ActionResult<IEnumerable<ProfileModel>>> GetUserProfile()
        {
            string? username = HttpContext.Items["Username"] as string;

            User? user = await db.Users.FirstOrDefaultAsync(x => x.username == username);
            ProfileModel profileModel = new ProfileModel()
            {
                username = user.username,
                first_name = user.first_name,
                last_name = user.last_name,
                email = user.email,
                phone_number = user.phone_number,
                coins = user.coins,
                country_id = user.country_id
            };
            return Ok(profileModel);
        }
    }
}
