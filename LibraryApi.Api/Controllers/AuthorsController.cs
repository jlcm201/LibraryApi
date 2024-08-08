using LibraryApi.Api.Models;
using LibraryApi.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService autorService)
        {
            _authorService = autorService;
        }

        // GET: /Autores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> GetAuthors()
        {
            var autores = await _authorService.GetAuthorsAsync();
            return Ok(autores);
        }
    }
}
