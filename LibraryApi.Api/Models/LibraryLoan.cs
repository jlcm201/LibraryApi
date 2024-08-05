namespace LibraryApi.Api.Models
{
    public class LibraryLoan
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public virtual Book? Book { get; set; }
        public virtual User? User { get; set; }
    }
}
