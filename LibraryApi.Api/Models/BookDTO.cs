namespace LibraryApi.Api.Models
{
    public class BookDTO
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public Guid AuthorId { get; set; }
        public string? Author { get; set; }
        public Guid CategoryId { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; } = string.Empty;
        public int Copies { get; set; }
    }
}
