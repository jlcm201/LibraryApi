using LibraryApi.Api.Models;

namespace LibraryApi.Api.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetUsersAsync();
    }
}
