namespace LibraryApi.Api.Models
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; } = string.Empty;
    }
}
