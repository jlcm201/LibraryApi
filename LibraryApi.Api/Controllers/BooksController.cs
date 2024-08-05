using LibraryApi.Api.Models;
using LibraryApi.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks()
        {
            var books = await _bookService.GetBooksAsync();
            return Ok(books);
        }

        // POST: Books
        [HttpPost]
        public async Task<ActionResult<BookDTO>> CreateBook(BookDTO bookDTO)
        {
            var createdBook = await _bookService.CreateBookAsync(bookDTO);
            return CreatedAtAction(nameof(GetBooks), new { id = createdBook.Id }, createdBook);
        }

        // DELETE: Books/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            var result = await _bookService.DeleteBookAsync(id);
            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
