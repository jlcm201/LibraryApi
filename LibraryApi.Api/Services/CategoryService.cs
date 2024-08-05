using LibraryApi.Api.Data;
using LibraryApi.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly LibraryContext _context;

        public CategoryService(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync()
        {
            return await _context.Categories
            .Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name, 
                Description = c.Description
            })
            .ToListAsync();
        }
    }
}
