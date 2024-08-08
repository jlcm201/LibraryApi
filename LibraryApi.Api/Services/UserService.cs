using LibraryApi.Api.Data;
using LibraryApi.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Api.Services
{
    public class UserService : IUserService
    {
        private readonly LibraryContext _context;

        public UserService(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            return await _context.Users
            .Select(u => new UserDTO
            {
                Id = u.Id,
                Name = $"{u.SecondName}, {u.Name}"
            })
            .ToListAsync();
        }
    }
}
