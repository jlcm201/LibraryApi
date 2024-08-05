using LibraryApi.Api.Data;
using LibraryApi.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Api.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly LibraryContext _context;

        public AuthorService(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AuthorDTO>> GetAuthorsAsync()
        {
            return await _context.Authors
            .Select(a => new AuthorDTO
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description
            })
            .ToListAsync();
        }
    }
}
