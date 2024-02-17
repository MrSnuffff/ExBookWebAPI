using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExBookWebAPI.Data;
using ExBookWebAPI.Models;
using ExBookWebAPI.OtherAPIs;

namespace ExBookWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserbookController : Controller
    {
        private readonly SearchBookInOtherAPIs _searchInOtherAPIs;
        private readonly AppDbContext db;
        public UserbookController(AppDbContext context, SearchBookInOtherAPIs searchInOtherAPIs)
        {
            db = context;
            _searchInOtherAPIs = searchInOtherAPIs;
        }
        //ok
        [HttpPost]
        [Route("AddUserBookViaIsbnAndDb")]
        public async Task<ActionResult<string>> AddUserBookViaIsbnAndDb(string isbn, string description)
        {
            if (isbn != null)
                isbn = isbn.Replace("-", "");
            string username = HttpContext.Items["Username"] as string;

            User? user = await db.Users.FirstOrDefaultAsync(x => x.username == username);
            Book? existingBook = await db.Books.FirstOrDefaultAsync(x => x.isbn == isbn);
            if (existingBook != null)
            {
                Userbook userbook = new Userbook { user_id = user.user_id, book_id = existingBook.book_id, description = description };
                db.Userbooks.Add(userbook);
                await db.SaveChangesAsync();
                return Ok();
            }
            return BadRequest("Тhe book does not exist in the database");
        }

        //ok
        [HttpPost]
        [Route("AddUserBookViaIsbnAndOtherApi")]
        public async Task<ActionResult<string>> AddUserBookViaIsbnAndOtherApi(string isbn, string description)
        {
            if (isbn != null)
                isbn = isbn.Replace("-", "");

            if (db.Books.Any(x => x.isbn == isbn))
            {
                return Conflict("Book already exists");
            }
            string username = HttpContext.Items["Username"] as string;

            User? user = await db.Users.FirstOrDefaultAsync(x => x.username == username);
            Book? BookFromAnotherApi = await _searchInOtherAPIs.SearchBookAsync(isbn);
            if (BookFromAnotherApi != null)
            {

                Userbook userbook = new Userbook { user_id = user.user_id, Book = BookFromAnotherApi, description = description };
                db.Userbooks.Add(userbook);

                await db.SaveChangesAsync();
                return Ok();
            }
            return BadRequest("Тhe book does not exist in other api");


        }

        //ok
        [HttpPost]
        [Route("AddUserBookViaTitleAndDb")]
        public async Task<ActionResult<string>> AddUserBookViaTitleAndDb(string title, string description)
        {

            string username = HttpContext.Items["Username"] as string;

            User? user = await db.Users.FirstOrDefaultAsync(x => x.username == username);
            Book? Book = await db.Books.FirstOrDefaultAsync(x => x.title == title);
            if (Book != null)
            {
                Userbook userbook = new Userbook { user_id = user.user_id, book_id = Book.book_id, description = description };
                db.Userbooks.Add(userbook);
                await db.SaveChangesAsync();
                return Ok();
            }
            return BadRequest("Тhe book does not exist in db");
        }

        //ok
        [HttpPost]
        [Route("AddUserBookViaModel")]
        public async Task<ActionResult<string>> AddUserBookViaModel(Userbook userbook)
        {
            if (db.Books.Any(x => x.isbn == userbook.Book.isbn || x.title == userbook.Book.title))
            {
                return Conflict("Book already exists");
            }

            if (userbook.Book.isbn != null)
                userbook.Book.isbn = userbook.Book.isbn.Replace("-", "");

            string username = HttpContext.Items["Username"] as string;

            User? user = await db.Users.FirstOrDefaultAsync(x => x.username == username);
            userbook.user_id = user.user_id;
            db.Userbooks.Add(userbook);
            await db.SaveChangesAsync();
            return Ok();
        }

        //ok
        [HttpGet]
        [Route("GetAllUserBooks")]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllUserBooks()
        {

            string username = HttpContext.Items["Username"] as string;

            User? user = await db.Users.FirstOrDefaultAsync(x => x.username == username);

            List<int> book_ids = await db.Userbooks
                   .Where(ub => ub.user_id == user.user_id)
                   .Select(ub => ub.book_id)
                   .ToListAsync();

            if (book_ids.Any())
            {
                List<Book> books = await db.Books
                    .Where(b => book_ids.Contains(b.book_id))
                    .ToListAsync();

                return Ok(books);
            }

            return NotFound("User doesn't have any books.");


        }
    }
}
