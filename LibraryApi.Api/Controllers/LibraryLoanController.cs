using LibraryApi.Api.Models;
using LibraryApi.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LibraryLoanController : ControllerBase
    {
        private readonly ILibraryLoanService _libraryLoanService;

        public LibraryLoanController(ILibraryLoanService libraryLoanService)
        {
            _libraryLoanService = libraryLoanService;
        }

        // GET: /LibraryLoan
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibraryLoanDTO>>> GetLibraryLoans()
        {
            var lobraryLoans = await _libraryLoanService.GetLibraryLoanAsync();
            return Ok(lobraryLoans);
        }

        // GET: /LibraryLoan/user/{id}
        [HttpGet("user/{id}")]
        public async Task<ActionResult<LibraryLoanDTO>> GetLibraryLoansByUserId(Guid id)
        {
            var libraryLoans = await _libraryLoanService.GetLibraryLoansByUserId(id);

            if (libraryLoans.ToList().Count == 0) return NotFound();

            return Ok(libraryLoans);
        }

        // POST: /LibraryLoan
        [HttpPost]
        public async Task<ActionResult<LibraryLoanDTO>> CreateLibraryLoan(LibraryLoanDTO libraryLoanDTO)
        {
            var bookIsAvailable = await _libraryLoanService.GetBookAvailabilityAsync(libraryLoanDTO.BookId);
            if (!bookIsAvailable) return NotFound();

            var createdLibraryLoan = await _libraryLoanService.CreateLibraryLoanAsync(libraryLoanDTO);

            return CreatedAtAction(nameof(GetLibraryLoans), new { id = createdLibraryLoan.Id }, createdLibraryLoan);
        }

        // DELETE: /LibraryLoan/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> ReturnLibraryLoan(Guid id)
        {
            var result = await _libraryLoanService.DeleteLibraryLoanAsync(id);

            if (result)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}
