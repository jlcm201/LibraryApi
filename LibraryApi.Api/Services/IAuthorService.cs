using LibraryApi.Api.Models;

namespace LibraryApi.Api.Services
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorDTO>> GetAuthorsAsync();
    }
}
