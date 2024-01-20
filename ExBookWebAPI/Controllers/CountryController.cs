using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExBookWebAPI.Data;
using ExBookWebAPI.Models;

namespace ExBookWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : Controller
    {
        private readonly AppDbContext db;
        public CountryController( AppDbContext context) 
        {
            db = context;
        }

        [HttpGet]
        [Route("GetAllCounties")]
        public async Task<ActionResult<IEnumerable<Country>>> GetAllCountries()
        {
            return await db.Countries.ToListAsync();
        }
    }
}
