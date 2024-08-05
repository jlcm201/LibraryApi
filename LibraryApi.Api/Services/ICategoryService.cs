using LibraryApi.Api.Models;

namespace LibraryApi.Api.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetCategoriesAsync();
    }
}
