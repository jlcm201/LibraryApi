namespace LibraryApi.Api.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? DeletedOn { get; set; } = null;

        public string Title { get; set; }
        public string? Description { get; set; } = string.Empty;
        public int Copies { get; set; }
        public Guid AuthorId { get; set; }
        public Guid CategoryId { get; set; }
        public virtual Author? Author { get; set; }
        public virtual Category? Category { get; set; }
    }
}
