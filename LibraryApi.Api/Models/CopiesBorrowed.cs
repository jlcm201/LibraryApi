namespace LibraryApi.Api.Models
{
    public class CopiesBorrowed
    {
        public Guid BookId { get; set; }
        public int Borrowed { get; set; }
    }
}
