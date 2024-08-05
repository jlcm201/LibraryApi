using LibraryApi.Api.Models;

namespace LibraryApi.Api.Services
{
    public interface IBookService
    {
        Task<IEnumerable<BookDTO>> GetBooksAsync();
        Task<BookDTO> CreateBookAsync(BookDTO bookDTO);
        Task<bool> DeleteBookAsync(Guid id);
    }
}
