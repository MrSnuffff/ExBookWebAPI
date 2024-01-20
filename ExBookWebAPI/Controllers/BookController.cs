using Microsoft.AspNetCore.Mvc;
using ExBookWebAPI.Data;
using ExBookWebAPI.OtherAPIs;

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

       

    }
}
