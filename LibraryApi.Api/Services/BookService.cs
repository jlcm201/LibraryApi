using LibraryApi.Api.Data;
using LibraryApi.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Api.Services
{
    public class BookService : IBookService
    {
        private readonly LibraryContext _context;

        public BookService(LibraryContext context)
        {
            _context = context;
        }

        public async Task<BookDTO> CreateBookAsync(BookDTO bookDTO)
        {
            var book = new Book
            {
                Title = bookDTO.Title,
                AuthorId = bookDTO.AuthorId,
                CategoryId = bookDTO.CategoryId,
                CreatedOn = DateTime.Now,
                Description = bookDTO.Description,
                Copies = bookDTO.Copies
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author!.Name,
                Category = book.Category!.Name,
                Description = book.Description,
                Copies = book.Copies
            };
        }

        public async Task<bool> DeleteBookAsync(Guid id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return false;
            }

            book.DeletedOn = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<BookDTO>> GetBooksAsync()
        {
            var copiesBorrowed = await _context.LibraryLoans
            .Where(p => p.ReturnDate == null)
            .GroupBy(p => p.BookId)
            .Select(p => new CopiesBorrowed
            {
                BookId = p.Key,
                Borrowed = p.Count()
            })
            .ToListAsync();

            return await _context.Books
            .Include(l => l.Author)
            .Include(l => l.Category)
            .Where(l => l.DeletedOn == null)
            .Select(l => ReturnBookDTO(l, copiesBorrowed))
            .ToListAsync();
        }

        private BookDTO ReturnBookDTO(Book book, IEnumerable<CopiesBorrowed> copiesBorrowed)
        {
            var _copiesBorrowed = copiesBorrowed!.FirstOrDefault(x => x.BookId == book.Id)! != null ?
                copiesBorrowed.FirstOrDefault(c => c.BookId == book.Id)!.Borrowed
                : 0;
            return new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                AuthorId = book.AuthorId,
                Author = book.Author!.Name,
                CategoryId = book.CategoryId,
                Category = book.Category!.Name,
                Description = book.Description,
                Copies = book.Copies - _copiesBorrowed,
            };
        }
    }
}
