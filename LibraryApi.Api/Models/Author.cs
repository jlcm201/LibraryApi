namespace LibraryApi.Api.Models
{
    public class Author
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? DeletedOn { get; set; } = null;
        public string Name{ get; set; }
        public string? Description { get; set; } = string.Empty;

    }
}
