namespace LibraryApi.Api.Models
{
    public class LibraryLoanDTO
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public string? BookTitle { get; set; }
        public string? BookAuthor { get; set; }
        public string? BookCategory { get; set; }
        public int AvailableCopies { get; set; }
        public Guid UserId { get; set; }
        public string? User { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
