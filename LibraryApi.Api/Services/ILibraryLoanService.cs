
using LibraryApi.Api.Models;

namespace LibraryApi.Api.Services
{
    public interface ILibraryLoanService
    {
        Task<IEnumerable<LibraryLoanDTO>> GetLibraryLoanAsync();
        Task<IEnumerable<LibraryLoanDTO>> GetLibraryLoansByUserId(Guid id);
        Task<LibraryLoanDTO> CreateLibraryLoanAsync(LibraryLoanDTO libraryLoanDTO);
        Task<bool> GetBookAvailabilityAsync(Guid id);
        Task<bool> DeleteLibraryLoanAsync(Guid id);
    }
}
