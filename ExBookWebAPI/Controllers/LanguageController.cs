using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExBookWebAPI.Data;
using ExBookWebAPI.Models;

namespace ExBookWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LanguageController : Controller
    {
        private readonly AppDbContext db;
        public LanguageController(AppDbContext context)
        {
            db = context;
        }

        [HttpGet]
        [Route("GelAllLanguages")]
        public async Task<ActionResult<IEnumerable<Language>>> GelAllLanguages()
        {
            return await db.Languages.ToListAsync();
        }
    }
}
