using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ExBookWebAPI.Data;
using ExBookWebAPI.Models;

namespace ExBookWebAPI.OtherAPIs
{
    public class SearchBookInOtherAPIs
    {
        private readonly AppDbContext db;
        private readonly HttpClient _httpClient;

        public SearchBookInOtherAPIs(AppDbContext context, HttpClient httpClient)
        {
            db = context;
            _httpClient = httpClient;
        }

        public async Task<Book?> SearchBookAsync(string isbn)
        {
            string url = $"https://openlibrary.org/isbn/{isbn}.json";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    Book book = new Book();
                    Author author = new Author();

                    string responseBody = await response.Content.ReadAsStringAsync();

                    JObject bookObject = JObject.Parse(responseBody);

                    string? title = bookObject["title"].ToString();
                    string? language = bookObject["languages"]?[0]?["key"]?.ToString()?.Substring(11);
                    string? authorKey = bookObject["authors"]?[0]?["key"]?.ToString()?.Substring(9);
                    string? publisherName = bookObject["publishers"]?[0].ToString();
                    string? bookKey = bookObject["key"].ToString();

                    if (!string.IsNullOrEmpty(authorKey))
                    {
                        author = await GetAuthorInfoAsync(authorKey);
                        Author existingAuthor = await db.Authors.FirstOrDefaultAsync(x => x.author_name == author.author_name);

                        if (existingAuthor != null)
                            book.author_id = existingAuthor.author_id;
                        else
                            book.Author = new Author { author_name = author.author_name };
                    }

                    if (!string.IsNullOrEmpty(language))
                    {
                        Language existingLanguage = await db.Languages.FirstOrDefaultAsync(x => x.language_code == language);
                        if (existingLanguage != null)
                            book.language_id = existingLanguage.language_id;
                    }

                    if (!string.IsNullOrEmpty(publisherName))
                    {
                        Publisher existingPublisher = await db.Publishers.FirstOrDefaultAsync(x => x.publisher_name == publisherName);

                        if (existingPublisher != null)
                            book.publisher_id = existingPublisher.publisher_id;
                        else
                            book.Publisher = new Publisher { publisher_name = publisherName };
                    }

                    book.isbn = isbn;
                    book.title = title;

                    return book;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        
        public async Task<Author?> GetAuthorInfoAsync(string authorKey)
        {
            string apiUrl = $"https://openlibrary.org/authors/{authorKey}.json";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    JObject AuthorObject = JObject.Parse(responseBody);

                    Author author = new Author { author_name = AuthorObject["name"].ToString() };
                    return author;
                }
                else
                    return null;
            }
            catch 
            {
                return null;
            }
        }
    }
}
