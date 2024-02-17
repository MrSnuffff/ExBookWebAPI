using Microsoft.AspNetCore.Mvc;
using ExBookWebAPI.Data;
using ExBookWebAPI.OtherAPIs;
using ExBookWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ExBookWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : Controller
    {

        private readonly AppDbContext db;
        private readonly SearchBookInOtherAPIs _searchInOtherAPIs;

        public BookController(AppDbContext context, SearchBookInOtherAPIs searchInOtherAPIs)
        {
            db = context;
            _searchInOtherAPIs = searchInOtherAPIs;
        }

        [HttpGet]
        [Route("GetBooksViaID")]
        public async Task<ActionResult<Book>> GetBooksViaID(uint book_id)
        {
            Book? book = await db.Books.FirstOrDefaultAsync(x => x.book_id == book_id);
            return book;
        }

    }
}