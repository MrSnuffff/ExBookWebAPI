using Microsoft.AspNetCore.Mvc;
using ExBookWebAPI.Data;
using ExBookWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ExBookWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchUserBookController : Controller
    {
        private readonly AppDbContext db;
        public SearchUserBookController(AppDbContext context)
        {
            db = context;
        }

        [HttpGet]
        [Route("Getbooks")]
        public async Task<ActionResult<IEnumerable<Userbook>>> GetUserbooks(uint page = 1, uint pageSize = 20, string isbn = null, string title = null)
        {
            IQueryable<Userbook> query = db.Userbooks.OrderBy(ub => ub.userbook_id);

            if (!string.IsNullOrEmpty(isbn))
                query = query.Where(ub => ub.Book.isbn == isbn);
            
            else if (!string.IsNullOrEmpty(title))
                query = query.Where(ub => ub.Book.title.Contains(title));
            

            var books = await query
                .Skip((int)((page - 1) * pageSize))
                .Take((int)pageSize)
                .ToListAsync();

            return books;
        }
    }
}
