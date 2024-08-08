using LibraryApi.Api.Data;
using LibraryApi.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace LibraryApi.Api.Services
{
    public class LibraryLoanService : ILibraryLoanService
    {
        private readonly LibraryContext _context;

        public LibraryLoanService(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LibraryLoanDTO>> GetLibraryLoanAsync()
        {
            return await _context.LibraryLoans
                .Include(p => p.Book)
                .ThenInclude(l => l!.Author)
                .Include(p => p.Book)
                .ThenInclude(l => l!.Category)
                .Include(p => p.User)
                .Where(p => p.ReturnDate == null)
                .Select(p => new LibraryLoanDTO
                {
                    Id = p.Id,
                    BookId = p.BookId,
                    BookTitle = p.Book!.Title,
                    BookAuthor = p.Book!.Author!.Name,
                    BookCategory = p.Book!.Category!.Name,
                    UserId = p.UserId,
                    User = $"{p.User!.SecondName}, {p.User!.Name}",
                    LoanDate = p.LoanDate,
                    ReturnDate = p.ReturnDate
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<LibraryLoanDTO>> GetLibraryLoansByUserId(Guid id)
        {
            var libraryLoansDTO = await _context.LibraryLoans
                .Include(p => p.Book)
                .Include(p => p.User)
                .Where(p => p.UserId == id)
                .Where(p => p.ReturnDate == null)
                .Select(p => new LibraryLoanDTO
                {
                    Id = p.Id,
                    BookId = p.BookId,
                    BookTitle = p.Book!.Title,
                    BookAuthor = p.Book!.Author!.Name,
                    BookCategory = p.Book!.Category!.Name,
                    UserId= p.UserId,
                    User = $"{p.User!.SecondName}, {p.User!.Name}",
                    LoanDate = p.LoanDate,
                    ReturnDate = p.ReturnDate
                })
                .ToListAsync();

            if (libraryLoansDTO == null) return new List<LibraryLoanDTO>();

            return libraryLoansDTO;
        }

        public async Task<LibraryLoanDTO> CreateLibraryLoanAsync(LibraryLoanDTO libraryLoanDTO)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == libraryLoanDTO.BookId);
            if (book != null)
            { 
                book.Author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == book.AuthorId);
                book.Category = await _context.Categories.FirstOrDefaultAsync(a => a.Id == book.CategoryId);
            }
            var libraryLoan = new LibraryLoan
            {
                BookId = libraryLoanDTO.BookId,
                Book = book,
                UserId = libraryLoanDTO.UserId,
                User = await _context.Users.FirstOrDefaultAsync(a => a.Id == libraryLoanDTO.UserId),
            LoanDate = DateTime.Now
            };

            _context.LibraryLoans.Add(libraryLoan);
            await _context.SaveChangesAsync();

            return ReturnLibraryLoanDTO(libraryLoan);
        }

        public async Task<bool> DeleteLibraryLoanAsync(Guid id)
        {
            var libraryLoan = await _context.LibraryLoans.FindAsync(id);

            if (libraryLoan == null)
            {
                return false;
            }

            libraryLoan.ReturnDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        private LibraryLoanDTO ReturnLibraryLoanDTO(LibraryLoan libraryLoan)
        {
            return new LibraryLoanDTO
            {
                Id = libraryLoan.Id,
                BookId = libraryLoan.BookId,
                BookTitle = libraryLoan.Book!.Title,
                BookAuthor = libraryLoan.Book.Author!.Name,
                BookCategory = libraryLoan.Book!.Category!.Name,
                UserId = libraryLoan.UserId,
                User = $"{libraryLoan.User!.SecondName}, {libraryLoan.User!.Name}",
                LoanDate = libraryLoan.LoanDate,
                ReturnDate = libraryLoan.ReturnDate
            };
        }

        public async Task<bool> GetBookAvailabilityAsync(Guid id)
        {
            var copiesBorrowed = await _context.LibraryLoans
                .Where(p => p.ReturnDate == null)
                .Where(p => p.BookId == id)
                .Select(p => new
                {
                    BookId = p.BookId
                })
                .ToListAsync();

            var book = await _context.Books
                .Where(p => p.DeletedOn == null)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            var availableCopies = (book != null ? book!.Copies : 0) - copiesBorrowed.Count;

            if (availableCopies > 0) return true;
            return false;
        }
    }
}
